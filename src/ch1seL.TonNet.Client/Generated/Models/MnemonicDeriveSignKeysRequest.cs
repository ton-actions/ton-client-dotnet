using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ch1seL.TonNet.Client.Models
{
    public class MnemonicDeriveSignKeysRequest
    {
        [JsonPropertyName("phrase")]
        public string Phrase { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("dictionary"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte? Dictionary { get; set; }
        [JsonPropertyName("word_count"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte? WordCount { get; set; }
    }
}