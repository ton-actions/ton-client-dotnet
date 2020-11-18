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
    public abstract class StateInitSource
    {
        [JsonDiscriminator("Message")]
        /// <summary>
        ///  Deploy message.
        /// </summary>
        public class Message : StateInitSource
        {
            /// <summary>
            ///  Deploy message.
            /// </summary>
            [JsonPropertyName("source")]
            public MessageSource Source { get; set; }
        }

        [JsonDiscriminator("StateInit")]
        /// <summary>
        ///  State init data.
        /// </summary>
        public class StateInit : StateInitSource
        {
            /// <summary>
            ///  State init data.
            /// </summary>
            [JsonPropertyName("code")]
            public string Code { get; set; }

            /// <summary>
            ///  State init data.
            /// </summary>
            [JsonPropertyName("data")]
            public string Data { get; set; }

            /// <summary>
            ///  State init data.
            /// </summary>
            [JsonPropertyName("library")]
            public string Library { get; set; }
        }

        [JsonDiscriminator("Tvc")]
        /// <summary>
        ///  Content of the TVC file. Encoded in `base64`.
        /// </summary>
        public class Tvc : StateInitSource
        {
            /// <summary>
            ///  Content of the TVC file. Encoded in `base64`.
            /// </summary>
            [JsonPropertyName("tvc")]
            public string TvcAccessor { get; set; }

            /// <summary>
            ///  Content of the TVC file. Encoded in `base64`.
            /// </summary>
            [JsonPropertyName("public_key")]
            public string PublicKey { get; set; }

            /// <summary>
            ///  Content of the TVC file. Encoded in `base64`.
            /// </summary>
            [JsonPropertyName("init_params")]
            public StateInitParams InitParams { get; set; }
        }
    }
}