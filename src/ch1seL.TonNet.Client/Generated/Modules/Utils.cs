using ch1seL.TonNet.Abstract;
using ch1seL.TonNet.Client.Abstract;
using ch1seL.TonNet.Client.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ch1seL.TonNet.Client
{
    public class Utils : IUtils
    {
        private readonly ITonClientAdapter _tonClientAdapter;

        public Utils(ITonClientAdapter tonClientAdapter)
        {
            _tonClientAdapter = tonClientAdapter;
        }

        public async Task<ConvertAddressResponse> ConvertAddress(ConvertAddressRequest @params, CancellationToken cancellationToken = default)
        {
            return await _tonClientAdapter.Request<ConvertAddressRequest, ConvertAddressResponse>("utils.convert_address", @params, cancellationToken);
        }
    }
}