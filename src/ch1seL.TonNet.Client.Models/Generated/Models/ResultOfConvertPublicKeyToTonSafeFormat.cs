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
    public class ResultOfConvertPublicKeyToTonSafeFormat
    {
        /// <summary>
        /// Public key represented in TON safe format.
        /// </summary>
        [JsonPropertyName("ton_public_key")]
        public string TonPublicKey { get; set; }
    }
}