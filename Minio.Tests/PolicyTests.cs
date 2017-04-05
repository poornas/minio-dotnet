﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.DataModel;
using Minio.DataModel.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Minio.Tests
{
    [TestClass]
    public class PolicyTests
    {

        [TestMethod]
        public void TestConditionKeyMapAdd()
        {
            var testCases = new List<KeyValuePair<Tuple<string, HashSet<string>>, string>>()
            {
                // Add new k-v pair
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:prefix",new HashSet<string>(){"hello" }), @"{""s3:prefix"":[""hello""]}" ),
               // Add existing k-v pair
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:prefix",new HashSet<string>(){"hello" }), @"{""s3:prefix"":[""hello""]}" ),
               // Add existing key and not value
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:prefix",new HashSet<string>(){"world" }), @"{""s3:prefix"":[""hello"", ""world""]}" ),

            };
            ConditionKeyMap cmap = new ConditionKeyMap();
            int index = 0;
            foreach (KeyValuePair<Tuple<string, HashSet<string>>, string> pair in testCases)
            {
                try
                {
                    index += 1;
                    var testcase = pair.Key;
                    string prefix = testcase.Item1;
                    HashSet<string> stringSet = testcase.Item2;
                    string expectedConditionKMap = pair.Value;
                    cmap.Add(prefix, stringSet);
                    string cmpstring = JsonConvert.SerializeObject(cmap, Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                    Assert.AreEqual(cmpstring, expectedConditionKMap);
                }
                catch (ArgumentException)
                {
                    Assert.AreNotEqual(index, 1);
                }
               
               
            }
        }

        [TestMethod]
        public void TestConditionKeyMapRemove()
        {
            var testCases = new List<KeyValuePair<Tuple<string, HashSet<string>>, string>>()
            {
                // Add new k-v pair
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:myprefix",new HashSet<string>(){"hello" }), @"{""s3:prefix"":[""hello"",""world""]}" ),
               // Add existing k-v pair
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:prefix",new HashSet<string>(){"hello" }), @"{""s3:prefix"":[""world""]}" ),
               // Add existing key and not value
               new KeyValuePair<Tuple<string, HashSet<string>>, string>(new Tuple<string,HashSet<string>>("s3:prefix",new HashSet<string>(){"world" }), @"{}" ),

            };
            ConditionKeyMap cmap = new ConditionKeyMap();
            cmap.Add("s3:prefix", new HashSet<string>() {"hello" , "world" });

            int index = 0;
            foreach (KeyValuePair<Tuple<string, HashSet<string>>, string> pair in testCases)
            {
                try
                {
                    index += 1;
                    var testcase = pair.Key;
                    string prefix = testcase.Item1;
                    HashSet<string> stringSet = testcase.Item2;
                    string expectedConditionKMap = pair.Value;
                    cmap.remove(prefix, stringSet);
                    string cmpstring = JsonConvert.SerializeObject(cmap, Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                    Assert.AreEqual(cmpstring, expectedConditionKMap);
                }
                catch (ArgumentException)
                {
                    Assert.AreNotEqual(index, 1);
                }


            }
        }

        [TestMethod]
        // Tests if condition key map merges existing values 
        public void TestConditionKeyMapPut()
        {
            ConditionKeyMap cmap1 = new ConditionKeyMap();
            cmap1.Add("s3:prefix", new HashSet<string>() { "hello" });

            ConditionKeyMap cmap2 = new ConditionKeyMap();
            cmap2.Add("s3:prefix", new HashSet<string>() { "world" });

            ConditionKeyMap cmap3 = new ConditionKeyMap();
            cmap3.Add("s3:myprefix", new HashSet<string>() { "world" });

            ConditionKeyMap cmap4 = new ConditionKeyMap();
            cmap4.Add("s3:prefix", new HashSet<string>() { "hello" });
            var testCases = new List<KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>>()
            {
                // Both args are empty
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(new ConditionKeyMap(),new ConditionKeyMap()), @"{}" ),
               // First arg empty
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(new ConditionKeyMap(),cmap1), @"{""s3:prefix"":[""hello""]}" ),
               //Second arg empty
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(cmap1,new ConditionKeyMap()), @"{""s3:prefix"":[""hello""]}" ),
               //Both args have same value
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(cmap1,cmap4), @"{""s3:prefix"":[""hello""]}"),
               //Value of second arg will be merged
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(cmap1, cmap2), @"{""s3:prefix"":[""hello"",""world""]}" ),
               //second arg will be added 
               new KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string>(Tuple.Create(cmap1, cmap3), @"{""s3:prefix"":[""hello"",""world""],""s3:myprefix"":[""world""]}" ),

            };
         

            int index = 0;
            foreach (KeyValuePair<Tuple<ConditionKeyMap, ConditionKeyMap>, string> pair in testCases)
            {
                try
                {
                    index += 1;
                    var testcase = pair.Key;
                    ConditionKeyMap first = testcase.Item1;
                    ConditionKeyMap second = testcase.Item2;
                    string expectedConditionKMapJSON = pair.Value;
                    foreach (KeyValuePair<string, ISet<string>> kvpair in second)
                    {
                        first.Put(kvpair.Key, kvpair.Value);
                    }
                    string cmpstring = JsonConvert.SerializeObject(first, Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                    Assert.AreEqual(cmpstring, expectedConditionKMapJSON);
                }
                catch (ArgumentException)
                {
                    Assert.Fail();
                }


            }
        }
        [TestMethod]
        public void TestIfStatementIsValid()
        {
            var testCases = new List<KeyValuePair<string, bool>>()
            {
             new KeyValuePair<string, bool>("{}",true),

            };

            foreach (KeyValuePair<string, bool> pair in testCases)
            {

                string statementString = (string)pair.Key;
                bool isValid = pair.Value;
                Statement stmt = JsonConvert.DeserializeObject<Statement>(statementString, new StatementJsonConverter());
                Assert.AreEqual(isValid, stmt.isValid("my-bucket"));
            }
        }
        [TestMethod]
        public void TestIfStringIsetGetsDeSerialized_Test1()
        {
            string policyString = @"{""Version"":""2012 - 10 - 17"",""Statement"":[{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: GetBucketLocation"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt""},{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: ListBucket"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt"",""Condition"":{""StringEquals"":{""s3: prefix"":""dotnetcms1ssazhd""}}},{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: GetObject"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt / dotnetcms1ssazhd * ""}]}";


           // ConditionKeyMap ckmap = JsonConvert.DeserializeObject<ConditionKeyMap>(ckmapString);
            var contentBytes = System.Text.Encoding.UTF8.GetBytes(policyString);
            string bucketName = "miniodotnetvpn5pic718xfutt";
            var stream = new MemoryStream(contentBytes);
            BucketPolicy policy = BucketPolicy.parseJson(stream, bucketName);
        }
       
    }
}
