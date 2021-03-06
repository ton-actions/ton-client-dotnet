﻿using System;
using ch1seL.TonNet.ClientGenerator.Models;

namespace ch1seL.TonNet.ClientGenerator.Helpers
{
    internal static class ResultExtensions
    {
        public static string GetMethodReturnType(this Result result)
        {
            if (result.GenericName != ResultGenericName.ClientResult)
                throw new ArgumentOutOfRangeException(nameof(result.GenericName), result.GenericName, "Unsupported method result generic name");
            if (result.Type != ParamType.Generic) throw new ArgumentOutOfRangeException(nameof(Result.Type), result.Type, "Unsupported method result type");
            if (result.GenericArgs.Length != 1) throw new ArgumentException("Result should contains only one generic argument");

            GenericArg genericArg = result.GenericArgs[0];

            switch (genericArg.Type)
            {
                case GenericArgType.None:
                    return null;
                case GenericArgType.Ref:
                    return NamingConventions.Normalize(genericArg.RefName);
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), genericArg.Type, "Unsupported generic type");
            }
        }
    }
}