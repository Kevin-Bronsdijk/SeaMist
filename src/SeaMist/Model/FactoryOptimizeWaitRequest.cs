﻿using System;

namespace SeaMist.Model
{
    internal static class FactoryOptimizeWaitRequest
    {
        public static OptimizeWaitRequest Create(Uri imageUrl, IDataStore dataStore)
        {
            switch (dataStore.DataStoreName)
            {
                case "azure_store":
                    return new Azure.OptimizeWaitRequest(imageUrl, dataStore);
                case "s3_store":
                    return new S3.OptimizeWaitRequest(imageUrl, dataStore);
                default:
                    throw new InvalidOperationException($"Type {dataStore.DataStoreName} cannot be instantiated");
            }
        }
    }
}