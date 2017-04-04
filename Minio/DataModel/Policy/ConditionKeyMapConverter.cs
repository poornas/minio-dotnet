/*
 * Minio .NET Library for Amazon S3 Compatible Cloud Storage, (C) 2017 Minio, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Minio.DataModel.Policy
{

    public class ConditionKeyMapConverter  : JsonConverter
    {
        private bool foundKey = false;
        private readonly Type[] _types = { typeof(ConditionKeyMap) };
        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
           if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);

                List<StringSet> target = new List<StringSet>();

                serializer.Populate(jArray.CreateReader(), target);
                ConditionKeyMap ckmap = new ConditionKeyMap();
                foreach (StringSet set in target)
                {
                    Console.Out.WriteLine(set);
                    //TODO: Populate k-v pair..
                }
                return ckmap;
            }
            return new ConditionKeyMap();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}

