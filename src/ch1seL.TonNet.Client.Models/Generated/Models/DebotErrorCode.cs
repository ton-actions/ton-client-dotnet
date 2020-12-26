using Dahomey.Json.Attributes;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ch1seL.TonNet.Client.Models
{
    /// <summary>
    /// 

    /// </summary>
    public enum DebotErrorCode
    {
        DebotStartFailed = 801,
        DebotFetchFailed = 802,
        DebotExecutionFailed = 803,
        DebotInvalidHandle = 804
    }
}