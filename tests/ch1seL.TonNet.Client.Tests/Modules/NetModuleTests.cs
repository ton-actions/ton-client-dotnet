﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ch1seL.TonNet.Abstract;
using ch1seL.TonNet.Client.Models;
using ch1seL.TonNet.Client.Tests.Utils;
using ch1seL.TonNet.Serialization;
using ch1seL.TonNet.TestsShared;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ch1seL.TonNet.Client.Tests.Modules
{
    public class NetModuleTests : IClassFixture<TonClientTestsFixture>
    {
        private readonly TonClientTestsFixture _fixture;
        private readonly ITestOutputHelper _outputHelper;
        private readonly ITonClient _tonClient;

        public NetModuleTests(TonClientTestsFixture fixture, ITestOutputHelper outputHelper)
        {
            _fixture = fixture;
            _outputHelper = outputHelper;
            _tonClient = fixture.CreateClient(outputHelper, true);
        }

        private ITonClient GetNewClient()
        {
            return _fixture.CreateClient(_outputHelper, true);
        }

        [Fact]
        public async Task Query()
        {
            ResultOfQuery result = await _tonClient.Net.Query(new ParamsOfQuery
            {
                Query = "query{info{version}}"
            });

            var resultParsed = result.Result!.Value.ToAnonymous(new {data = new {info = new {version = default(string)}}});
            resultParsed.data.info.version.Split('.').Length.Should().Be(3);
        }

        [Fact]
        public async Task BlockSignatures()
        {
            ResultOfQueryCollection result = await _tonClient.Net.QueryCollection(new ParamsOfQueryCollection
            {
                Collection = "blocks_signatures",
                Filter = new { }.ToJsonElement(),
                Result = "id",
                Limit = 1
            });

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AllAccounts()
        {
            ResultOfQueryCollection result = await _tonClient.Net.QueryCollection(new ParamsOfQueryCollection
            {
                Collection = "accounts",
                Filter = new { }.ToJsonElement(),
                Result = "id balance"
            });

            result.Result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Ranges()
        {
            ResultOfQueryCollection result = await _tonClient.Net.QueryCollection(new ParamsOfQueryCollection
            {
                Collection = "messages",
                Filter = new {created_at = new {gt = 1562342740}}.ToJsonElement(),
                Result = "body created_at"
            });

            Assert.NotEmpty(result.Result);
            Assert.True(result.Result?[0].Get<ulong>("created_at") > 1562342740);
        }

        [Fact]
        public async Task WaitFor()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // get new client here to avoid lock another tests
            var request = GetNewClient().Net.WaitForCollection(new ParamsOfWaitForCollection
            {
                Collection = "transactions",
                Filter = new {now = new {gt = now}}.ToJsonElement(),
                Result = "id now"
            });

            await Task.Delay(TimeSpan.FromSeconds(1));

            await _tonClient.SendGramsFromLocalGiver();

            ResultOfWaitForCollection result = await request;
            result.Result.Get<long>("now").Should().BeGreaterThan(now);
        }

        [Fact]
        public async Task SubscribeForTransactionsWithAddresses()
        {
            KeyPair keys = await _tonClient.Crypto.GenerateRandomSignKeys();
            ITonClient subscriptionClient = _fixture.CreateClient(_outputHelper, true);

            var deployParams = new ParamsOfEncodeMessage
            {
                Abi = TestsEnv.Packages.Hello.Abi,
                DeploySet = new DeploySet {Tvc = TestsEnv.Packages.Hello.Tvc},
                Signer = new Signer.Keys {KeysAccessor = keys},
                CallSet = new CallSet {FunctionName = "constructor"}
            };

            ResultOfEncodeMessage msg = await _tonClient.Abi.EncodeMessage(deployParams);
            var transactions = new List<string>();
            var errorCodes = new List<uint>();
            var @lock = new object();

            var address = msg.Address;

            var callback = new Action<JsonElement, uint>((serdeJson, responseType) =>
            {
                switch ((SubscriptionResponseType) responseType)
                {
                    case SubscriptionResponseType.Ok:
                        JsonElement resultOk = serdeJson.GetProperty("result");
                        resultOk.Get<string>("account_addr").Should().Be(address);
                        lock (@lock)
                        {
                            transactions.Add(resultOk.Get<string>("id"));
                        }

                        break;
                    case SubscriptionResponseType.Error:
                        var error = serdeJson.ToObject<ClientError>();
                        _outputHelper.WriteLine($">> {error}");
                        lock (@lock)
                        {
                            errorCodes.Add(error.Code);
                        }

                        break;
                    default:
                        throw new TonClientException($"Unknown SubscriptionResponseType: {responseType}");
                }
            });

            //act
            ResultOfSubscribeCollection handle1 = await subscriptionClient.Net.SubscribeCollection(new ParamsOfSubscribeCollection
            {
                Collection = "transactions",
                Filter = new
                {
                    account_addr = new {eq = address},
                    status = new {eq = (int) TransactionProcessingStatus.Finalized}
                }.ToJsonElement(),
                Result = "id account_addr"
            }, callback);

            // send grams to create first transaction
            await _tonClient.SendGramsFromLocalGiver(address);

            // give some time for subscription to receive all data
            await Task.Delay(TimeSpan.FromSeconds(1));

            var transactionCount1 = transactions.Count;

            // suspend subscription
            await subscriptionClient.Net.Suspend();

            await Task.Delay(TimeSpan.FromSeconds(1));

            // deploy to create second transaction
            await _tonClient.Processing.ProcessMessage(new ParamsOfProcessMessage
            {
                MessageEncodeParams = deployParams,
                SendEvents = false
            });

            //act
            ResultOfSubscribeCollection handle2 = await subscriptionClient.Net.SubscribeCollection(new ParamsOfSubscribeCollection
            {
                Collection = "transactions",
                Filter = new
                {
                    account_addr = new {eq = address},
                    status = new {eq = (int) TransactionProcessingStatus.Finalized}
                }.ToJsonElement(),
                Result = "id account_addr"
            }, callback);

            await Task.Delay(TimeSpan.FromSeconds(1));

            // check that second transaction is not received when subscription suspended
            var transactionCount2 = transactions.Count;

            // resume subscription
            await subscriptionClient.Net.Resume();

            await Task.Delay(TimeSpan.FromSeconds(1));

            // run contract function to create third transaction
            await _tonClient.Processing.ProcessMessage(new ParamsOfProcessMessage
            {
                MessageEncodeParams = new ParamsOfEncodeMessage
                {
                    Abi = TestsEnv.Packages.Hello.Abi,
                    Signer = new Signer.Keys {KeysAccessor = keys},
                    Address = address,
                    CallSet = new CallSet {FunctionName = "touch"}
                },
                SendEvents = false
            });

            // give some time for subscription to receive all data
            await Task.Delay(TimeSpan.FromSeconds(1));

            await subscriptionClient.Net.Unsubscribe(new ResultOfSubscribeCollection
            {
                Handle = handle1.Handle
            });
            await subscriptionClient.Net.Unsubscribe(new ResultOfSubscribeCollection
            {
                Handle = handle2.Handle
            });

            //check count before suspending 
            transactionCount1.Should().Be(1);

            //check count before resume
            transactionCount2.Should().Be(1);

            // check that third transaction is now received after resume
            transactions.Count.Should().Be(3);
            transactions[0].Should().NotBe(transactions[2]);

            // check errors
            errorCodes.Count.Should().Be(4);
            errorCodes.Take(2).Should().AllBeEquivalentTo((uint) NetErrorCode.NetworkModuleSuspended);
            errorCodes.TakeLast(2).Should().AllBeEquivalentTo((uint) NetErrorCode.NetworkModuleResumed);
        }

        [Fact]
        public async Task SubscribeForMessages()
        {
            var messagesLock = new object();
            var messages = new List<JsonElement>();

            var callback = new Action<JsonElement, uint>((serdeJson, responseType) =>
            {
                JsonElement message = (ResponseType) responseType switch
                {
                    ResponseType.Custom => new {result = serdeJson}.ToJsonElement(),
                    _ => throw new TonClientException("bad callback gotten")
                };
                lock (messagesLock)
                {
                    messages.Add(message);
                }
            });

            //act
            ResultOfSubscribeCollection handle = await _tonClient.Net.SubscribeCollection(new ParamsOfSubscribeCollection
            {
                Collection = "messages",
                Filter = new {dst = new {eq = "1"}}.ToJsonElement(),
                Result = "id"
            }, callback);
            await _tonClient.SendGramsFromLocalGiver();
            await _tonClient.Net.Unsubscribe(new ResultOfSubscribeCollection
            {
                Handle = handle.Handle
            });

            // arrange
            messages.Count.Should().Be(0);
        }

        [Fact]
        public async Task FindLastShardBlock()
        {
            ResultOfFindLastShardBlock block = await _tonClient.Net.FindLastShardBlock(new ParamsOfFindLastShardBlock
            {
                Address = TestsEnv.LocalGiverAddress
            });

            block.BlockId.Should().NotBeNull();
            block.BlockId.Length.Should().Be(64);
        }

        // todo: not working yet https://t.me/ton_sdk/7063?thread=7032
        // [Fact]
        // public async Task TestEndpoints()
        // {
        //     ITonClient client = _fixture.CreateClient(_outputHelper,
        //         configureOptions: options => options.Network.Endpoints = new[] {"cinet.tonlabs.io", "cinet2.tonlabs.io/"});
        //
        //     Func<Task> act = async () =>
        //     {
        //         EndpointsSet endpoints = await client.Net.FetchEndpoints();
        //         await client.Net.SetEndpoints(endpoints);
        //     };
        //
        //     await act.Should().NotThrowAsync();
        // }
    }
}