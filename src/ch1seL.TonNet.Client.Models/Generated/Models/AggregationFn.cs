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
    public enum AggregationFn
    {
        COUNT,
        MIN,
        MAX,
        SUM,
        AVERAGE
    }
}