﻿using System.Text.Json.Serialization;

namespace ch1seL.TonNet.ClientGenerator.Models
{
    public class EnumType
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("type")] public GenericArgType Type { get; set; }

        [JsonPropertyName("struct_fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EnumType[] StructFields { get; set; }

        [JsonPropertyName("summary")] public string Summary { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }

        [JsonPropertyName("ref_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefName { get; set; }

        [JsonPropertyName("optional_inner")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GenericArg OptionalInner { get; set; }
        
        [JsonPropertyName("number_type")] public NumberType NumberType { get; set; }
        [JsonPropertyName("number_size")] public long NumberSize { get; set; }
    }
}