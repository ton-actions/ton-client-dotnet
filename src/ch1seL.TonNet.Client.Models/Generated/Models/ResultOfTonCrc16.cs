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
    public class ResultOfTonCrc16
    {
        /// <summary>
        /// Calculated CRC for input data.
        /// </summary>
        [JsonPropertyName("crc")]
        public ushort Crc { get; set; }
    }
}