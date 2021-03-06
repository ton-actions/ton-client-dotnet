using ch1seL.TonNet.Client.Models;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ch1seL.TonNet.Abstract.Modules
{
    public interface IUtilsModule : ITonModule
    {
        /// <summary>
        /// Converts address from any TON format to any TON format
        /// </summary>
        public Task<ResultOfConvertAddress> ConvertAddress(ParamsOfConvertAddress @params, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Validates and returns the type of any TON address.</para>
        /// <para>Address types are the following</para>
        /// <para>`0:919db8e740d50bf349df2eea03fa30c385d846b991ff5542e67098ee833fc7f7` - standart TON address most</para>
        /// <para>commonly used in all cases. Also called as hex addres</para>
        /// <para>`919db8e740d50bf349df2eea03fa30c385d846b991ff5542e67098ee833fc7f7` - account ID. A part of full</para>
        /// <para>address. Identifies account inside particular workchain</para>
        /// <para>`EQCRnbjnQNUL80nfLuoD+jDDhdhGuZH/VULmcJjugz/H9wam` - base64 address. Also called "user-friendly".</para>
        /// <para>Was used at the beginning of TON. Now it is supported for compatibility</para>
        /// </summary>
        public Task<ResultOfGetAddressType> GetAddressType(ParamsOfGetAddressType @params, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates storage fee for an account over a specified time period
        /// </summary>
        public Task<ResultOfCalcStorageFee> CalcStorageFee(ParamsOfCalcStorageFee @params, CancellationToken cancellationToken = default);

        /// <summary>
        /// Compresses data using Zstandard algorithm
        /// </summary>
        public Task<ResultOfCompressZstd> CompressZstd(ParamsOfCompressZstd @params, CancellationToken cancellationToken = default);

        /// <summary>
        /// Decompresses data using Zstandard algorithm
        /// </summary>
        public Task<ResultOfDecompressZstd> DecompressZstd(ParamsOfDecompressZstd @params, CancellationToken cancellationToken = default);
    }
}