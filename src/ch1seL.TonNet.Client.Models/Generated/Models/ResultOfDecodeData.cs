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
    public class ResultOfDecodeData
    {
        /// <summary>
        /// Decoded data as a JSON structure.
        /// </summary>
        [JsonPropertyName("data")]
        public JsonElement? Data { get; set; }
    }
}