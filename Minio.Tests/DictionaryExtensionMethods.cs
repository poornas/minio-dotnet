using System.Linq;
using System.Collections.Generic;
using Minio.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Minio.Tests
{
    public static class DictionaryExtensionMethods
    {
        public static bool ContentEquals<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> otherDictionary)
        {
            return (otherDictionary ?? new Dictionary<TKey, TValue>())
                .OrderBy(kvp => kvp.Key)
                .SequenceEqual((dictionary ?? new Dictionary<TKey, TValue>())
                                   .OrderBy(kvp => kvp.Key));
        }

        public static bool DictionaryEqual<String, PolicyType>(
        this IDictionary<String, PolicyType> first, IDictionary<String, PolicyType> second)
        { 
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;


            foreach (var kvp in first)
            {
                PolicyType secondValue;
                PolicyType firstValue = kvp.Value;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!firstValue.Equals(secondValue)) return false;
            }
            return true;
        }
    }
}

