using Dahomey.Json.Attributes;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ch1seL.TonNet.Client.Models
{
    /// <summary>
    /// Not described yet..
    /// </summary>
    public class ParamsOfChaCha20
    {
        /// <summary>
        /// <para>Source data to be encrypted or decrypted.</para>
        /// <para>Must be encoded with `base64`.</para>
        /// </summary>
        [JsonPropertyName("data")]
        public string Data { get; set; }

        /// <summary>
        /// <para>256-bit key.</para>
        /// <para>Must be encoded with `hex`.</para>
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// <para>96-bit nonce.</para>
        /// <para>Must be encoded with `hex`.</para>
        /// </summary>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }
    }
}