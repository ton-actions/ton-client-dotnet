using ch1seL.TonNet.Abstract;
using ch1seL.TonNet.Abstract.Modules;
using ch1seL.TonNet.Client.Models;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ch1seL.TonNet.Client.Modules
{
    public class NetModule : INetModule
    {
        private readonly ITonClientAdapter _tonClientAdapter;

        public NetModule(ITonClientAdapter tonClientAdapter)
        {
            _tonClientAdapter = tonClientAdapter;
        }

        /// <summary>
        /// Performs DAppServer GraphQL query.
        /// </summary>
        public async Task<ResultOfQuery> Query(ParamsOfQuery @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfQuery, ResultOfQuery>("net.query", @params, cancellationToken);
        }

        /// <summary>
        /// Performs multiple queries per single fetch.
        /// </summary>
        public async Task<ResultOfBatchQuery> BatchQuery(ParamsOfBatchQuery @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfBatchQuery, ResultOfBatchQuery>("net.batch_query", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Queries collection data</para>
        /// <para>Queries data that satisfies the `filter` conditions,</para>
        /// <para>limits the number of returned records and orders them.</para>
        /// <para>The projection fields are limited to `result` fields</para>
        /// </summary>
        public async Task<ResultOfQueryCollection> QueryCollection(ParamsOfQueryCollection @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfQueryCollection, ResultOfQueryCollection>("net.query_collection", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Aggregates collection data.</para>
        /// <para>Aggregates values from the specified `fields` for records</para>
        /// <para>that satisfies the `filter` conditions,</para>
        /// </summary>
        public async Task<ResultOfAggregateCollection> AggregateCollection(ParamsOfAggregateCollection @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfAggregateCollection, ResultOfAggregateCollection>("net.aggregate_collection", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Returns an object that fulfills the conditions or waits for its appearance</para>
        /// <para>Triggers only once.</para>
        /// <para>If object that satisfies the `filter` conditions</para>
        /// <para>already exists - returns it immediately.</para>
        /// <para>If not - waits for insert/update of data within the specified `timeout`,</para>
        /// <para>and returns it.</para>
        /// <para>The projection fields are limited to `result` fields</para>
        /// </summary>
        public async Task<ResultOfWaitForCollection> WaitForCollection(ParamsOfWaitForCollection @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfWaitForCollection, ResultOfWaitForCollection>("net.wait_for_collection", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Cancels a subscription</para>
        /// <para>Cancels a subscription specified by its handle.</para>
        /// </summary>
        public async Task Unsubscribe(ResultOfSubscribeCollection @params, CancellationToken cancellationToken = default)
        {
            await _tonClientAdapter.Request<ResultOfSubscribeCollection>("net.unsubscribe", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Creates a subscription</para>
        /// <para>Triggers for each insert/update of data that satisfies</para>
        /// <para>the `filter` conditions.</para>
        /// <para>The projection fields are limited to `result` fields.</para>
        /// <para>The subscription is a persistent communication channel between</para>
        /// <para>client and Free TON Network.</para>
        /// <para>All changes in the blockchain will be reflected in realtime.</para>
        /// <para>Changes means inserts and updates of the blockchain entities.</para>
        /// <para>### Important Notes on Subscriptions</para>
        /// <para>Unfortunately sometimes the connection with the network brakes down.</para>
        /// <para>In this situation the library attempts to reconnect to the network.</para>
        /// <para>This reconnection sequence can take significant time.</para>
        /// <para>All of this time the client is disconnected from the network.</para>
        /// <para>Bad news is that all blockchain changes that happened while</para>
        /// <para>the client was disconnected are lost.</para>
        /// <para>Good news is that the client report errors to the callback when</para>
        /// <para>it loses and resumes connection.</para>
        /// <para>So, if the lost changes are important to the application then</para>
        /// <para>the application must handle these error reports.</para>
        /// <para>Library reports errors with `responseType` == 101</para>
        /// <para>and the error object passed via `params`.</para>
        /// <para>When the library has successfully reconnected</para>
        /// <para>the application receives callback with</para>
        /// <para>`responseType` == 101 and `params.code` == 614 (NetworkModuleResumed).</para>
        /// <para>Application can use several ways to handle this situation:</para>
        /// <para>- If application monitors changes for the single blockchain</para>
        /// <para>object (for example specific account):  application</para>
        /// <para>can perform a query for this object and handle actual data as a</para>
        /// <para>regular data from the subscription.</para>
        /// <para>- If application monitors sequence of some blockchain objects</para>
        /// <para>(for example transactions of the specific account): application must</para>
        /// <para>refresh all cached (or visible to user) lists where this sequences presents.</para>
        /// </summary>
        public async Task<ResultOfSubscribeCollection> SubscribeCollection(ParamsOfSubscribeCollection @params, Action<JsonElement,uint> callback = null, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfSubscribeCollection, ResultOfSubscribeCollection, JsonElement>("net.subscribe_collection", @params, callback, cancellationToken);
        }

        /// <summary>
        /// Suspends network module to stop any network activity
        /// </summary>
        public async Task Suspend(CancellationToken cancellationToken = default)
        {
            await _tonClientAdapter.Request("net.suspend", cancellationToken);
        }

        /// <summary>
        /// Resumes network module to enable network activity
        /// </summary>
        public async Task Resume(CancellationToken cancellationToken = default)
        {
            await _tonClientAdapter.Request("net.resume", cancellationToken);
        }

        /// <summary>
        /// Returns ID of the last block in a specified account shard
        /// </summary>
        public async Task<ResultOfFindLastShardBlock> FindLastShardBlock(ParamsOfFindLastShardBlock @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfFindLastShardBlock, ResultOfFindLastShardBlock>("net.find_last_shard_block", @params, cancellationToken);
        }

        /// <summary>
        /// Requests the list of alternative endpoints from server
        /// </summary>
        public async Task<EndpointsSet> FetchEndpoints(CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<EndpointsSet>("net.fetch_endpoints", cancellationToken);
        }

        /// <summary>
        /// Sets the list of endpoints to use on reinit
        /// </summary>
        public async Task SetEndpoints(EndpointsSet @params, CancellationToken cancellationToken = default)
        {
            await _tonClientAdapter.Request<EndpointsSet>("net.set_endpoints", @params, cancellationToken);
        }

        /// <summary>
        /// Requests the list of alternative endpoints from server
        /// </summary>
        public async Task<ResultOfGetEndpoints> GetEndpoints(CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ResultOfGetEndpoints>("net.get_endpoints", cancellationToken);
        }

        /// <summary>
        /// <para>Allows to query and paginate through the list of accounts that the specified account has interacted with, sorted by the time of the last internal message between accounts</para>
        /// <para>*Attention* this query retrieves data from 'Counterparties' service which is not supported in</para>
        /// <para>the opensource version of DApp Server (and will not be supported) as well as in TON OS SE (will be supported in SE in future),</para>
        /// <para>but is always accessible via [TON OS Devnet/Mainnet Clouds](https://docs.ton.dev/86757ecb2/p/85c869-networks)</para>
        /// </summary>
        public async Task<ResultOfQueryCollection> QueryCounterparties(ParamsOfQueryCounterparties @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfQueryCounterparties, ResultOfQueryCollection>("net.query_counterparties", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Returns transactions tree for specific message.</para>
        /// <para>Performs recursive retrieval of the transactions tree produced by the specific message:</para>
        /// <para>in_msg -&gt; dst_transaction -&gt; out_messages -&gt; dst_transaction -&gt; ...</para>
        /// <para>All retrieved messages and transactions will be included</para>
        /// <para>into `result.messages` and `result.transactions` respectively.</para>
        /// <para>The retrieval process will stop when the retrieved transaction count is more than 50.</para>
        /// <para>It is guaranteed that each message in `result.messages` has the corresponding transaction</para>
        /// <para>in the `result.transactions`.</para>
        /// <para>But there are no guaranties that all messages from transactions `out_msgs` are</para>
        /// <para>presented in `result.messages`.</para>
        /// <para>So the application have to continue retrieval for missing messages if it requires.</para>
        /// </summary>
        public async Task<ResultOfQueryTransactionTree> QueryTransactionTree(ParamsOfQueryTransactionTree @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfQueryTransactionTree, ResultOfQueryTransactionTree>("net.query_transaction_tree", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Creates block iterator.</para>
        /// <para>Block iterator uses robust iteration methods that guaranties that every</para>
        /// <para>block in the specified range isn't missed or iterated twice.</para>
        /// <para>Iterated range can be reduced with some filters:</para>
        /// <para>- `start_time` – the bottom time range. Only blocks with `gen_utime`</para>
        /// <para>more or equal to this value is iterated. If this parameter is omitted then there is</para>
        /// <para>no bottom time edge, so all blocks since zero state is iterated.</para>
        /// <para>- `end_time` – the upper time range. Only blocks with `gen_utime`</para>
        /// <para>less then this value is iterated. If this parameter is omitted then there is</para>
        /// <para>no upper time edge, so iterator never finishes.</para>
        /// <para>- `shard_filter` – workchains and shard prefixes that reduce the set of interesting</para>
        /// <para>blocks. Block conforms to the shard filter if it belongs to the filter workchain</para>
        /// <para>and the first bits of block's `shard` fields matches to the shard prefix.</para>
        /// <para>Only blocks with suitable shard are iterated.</para>
        /// <para>Items iterated is a JSON objects with block data. The minimal set of returned</para>
        /// <para>fields is:</para>
        /// <para>```text</para>
        /// <para>id</para>
        /// <para>gen_utime</para>
        /// <para>workchain_id</para>
        /// <para>shard</para>
        /// <para>after_split</para>
        /// <para>after_merge</para>
        /// <para>prev_ref {</para>
        /// <para>    root_hash</para>
        /// <para>}</para>
        /// <para>prev_alt_ref {</para>
        /// <para>    root_hash</para>
        /// <para>}</para>
        /// <para>```</para>
        /// <para>Application can request additional fields in the `result` parameter.</para>
        /// <para>Application should call the `remove_iterator` when iterator is no longer required.</para>
        /// </summary>
        public async Task<RegisteredIterator> CreateBlockIterator(ParamsOfCreateBlockIterator @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfCreateBlockIterator, RegisteredIterator>("net.create_block_iterator", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Resumes block iterator.</para>
        /// <para>The iterator stays exactly at the same position where the `resume_state` was catched.</para>
        /// <para>Application should call the `remove_iterator` when iterator is no longer required.</para>
        /// </summary>
        public async Task<RegisteredIterator> ResumeBlockIterator(ParamsOfResumeBlockIterator @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfResumeBlockIterator, RegisteredIterator>("net.resume_block_iterator", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Creates transaction iterator.</para>
        /// <para>Transaction iterator uses robust iteration methods that guaranty that every</para>
        /// <para>transaction in the specified range isn't missed or iterated twice.</para>
        /// <para>Iterated range can be reduced with some filters:</para>
        /// <para>- `start_time` – the bottom time range. Only transactions with `now`</para>
        /// <para>more or equal to this value are iterated. If this parameter is omitted then there is</para>
        /// <para>no bottom time edge, so all the transactions since zero state are iterated.</para>
        /// <para>- `end_time` – the upper time range. Only transactions with `now`</para>
        /// <para>less then this value are iterated. If this parameter is omitted then there is</para>
        /// <para>no upper time edge, so iterator never finishes.</para>
        /// <para>- `shard_filter` – workchains and shard prefixes that reduce the set of interesting</para>
        /// <para>accounts. Account address conforms to the shard filter if</para>
        /// <para>it belongs to the filter workchain and the first bits of address match to</para>
        /// <para>the shard prefix. Only transactions with suitable account addresses are iterated.</para>
        /// <para>- `accounts_filter` – set of account addresses whose transactions must be iterated.</para>
        /// <para>Note that accounts filter can conflict with shard filter so application must combine</para>
        /// <para>these filters carefully.</para>
        /// <para>Iterated item is a JSON objects with transaction data. The minimal set of returned</para>
        /// <para>fields is:</para>
        /// <para>```text</para>
        /// <para>id</para>
        /// <para>account_addr</para>
        /// <para>now</para>
        /// <para>balance_delta(format:DEC)</para>
        /// <para>bounce { bounce_type }</para>
        /// <para>in_message {</para>
        /// <para>    id</para>
        /// <para>    value(format:DEC)</para>
        /// <para>    msg_type</para>
        /// <para>    src</para>
        /// <para>}</para>
        /// <para>out_messages {</para>
        /// <para>    id</para>
        /// <para>    value(format:DEC)</para>
        /// <para>    msg_type</para>
        /// <para>    dst</para>
        /// <para>}</para>
        /// <para>```</para>
        /// <para>Application can request an additional fields in the `result` parameter.</para>
        /// <para>Another parameter that affects on the returned fields is the `include_transfers`.</para>
        /// <para>When this parameter is `true` the iterator computes and adds `transfer` field containing</para>
        /// <para>list of the useful `TransactionTransfer` objects.</para>
        /// <para>Each transfer is calculated from the particular message related to the transaction</para>
        /// <para>and has the following structure:</para>
        /// <para>- message – source message identifier.</para>
        /// <para>- isBounced – indicates that the transaction is bounced, which means the value will be returned back to the sender.</para>
        /// <para>- isDeposit – indicates that this transfer is the deposit (true) or withdraw (false).</para>
        /// <para>- counterparty – account address of the transfer source or destination depending on `isDeposit`.</para>
        /// <para>- value – amount of nano tokens transferred. The value is represented as a decimal string</para>
        /// <para>because the actual value can be more precise than the JSON number can represent. Application</para>
        /// <para>must use this string carefully – conversion to number can follow to loose of precision.</para>
        /// <para>Application should call the `remove_iterator` when iterator is no longer required.</para>
        /// </summary>
        public async Task<RegisteredIterator> CreateTransactionIterator(ParamsOfCreateTransactionIterator @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfCreateTransactionIterator, RegisteredIterator>("net.create_transaction_iterator", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Resumes transaction iterator.</para>
        /// <para>The iterator stays exactly at the same position where the `resume_state` was caught.</para>
        /// <para>Note that `resume_state` doesn't store the account filter. If the application requires</para>
        /// <para>to use the same account filter as it was when the iterator was created then the application</para>
        /// <para>must pass the account filter again in `accounts_filter` parameter.</para>
        /// <para>Application should call the `remove_iterator` when iterator is no longer required.</para>
        /// </summary>
        public async Task<RegisteredIterator> ResumeTransactionIterator(ParamsOfResumeTransactionIterator @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfResumeTransactionIterator, RegisteredIterator>("net.resume_transaction_iterator", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Returns next available items.</para>
        /// <para>In addition to available items this function returns the `has_more` flag</para>
        /// <para>indicating that the iterator isn't reach the end of the iterated range yet.</para>
        /// <para>This function can return the empty list of available items but</para>
        /// <para>indicates that there are more items is available.</para>
        /// <para>This situation appears when the iterator doesn't reach iterated range</para>
        /// <para>but database doesn't contains available items yet.</para>
        /// <para>If application requests resume state in `return_resume_state` parameter</para>
        /// <para>then this function returns `resume_state` that can be used later to</para>
        /// <para>resume the iteration from the position after returned items.</para>
        /// <para>The structure of the items returned depends on the iterator used.</para>
        /// <para>See the description to the appropriated iterator creation function.</para>
        /// </summary>
        public async Task<ResultOfIteratorNext> IteratorNext(ParamsOfIteratorNext @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ParamsOfIteratorNext, ResultOfIteratorNext>("net.iterator_next", @params, cancellationToken);
        }

        /// <summary>
        /// <para>Removes an iterator</para>
        /// <para>Frees all resources allocated in library to serve iterator.</para>
        /// <para>Application always should call the `remove_iterator` when iterator</para>
        /// <para>is no longer required.</para>
        /// </summary>
        public async Task RemoveIterator(RegisteredIterator @params, CancellationToken cancellationToken = default)
        {
            await _tonClientAdapter.Request<RegisteredIterator>("net.remove_iterator", @params, cancellationToken);
        }
    }
}