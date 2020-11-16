using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ch1seL.TonNet.Client.Models
{
    public class CryptoConfig
    {
        [JsonPropertyName("mnemonic_dictionary"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte? MnemonicDictionary { get; set; }
        [JsonPropertyName("mnemonic_word_count"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte? MnemonicWordCount { get; set; }
        [JsonPropertyName("hdkey_derivation_path")]
        public string HdkeyDerivationPath { get; set; }
        [JsonPropertyName("hdkey_compliant"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HdkeyCompliant { get; set; }
    }
}