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
    public class ResultOfMnemonicFromEntropy
    {
        /// <summary>
        /// Phrase
        /// </summary>
        [JsonPropertyName("phrase")]
        public string Phrase { get; set; }
    }
}