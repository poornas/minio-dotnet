using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.DataModel;
using Minio.DataModel.Policy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Minio.Tests
{
    class TestHelper
    {
        private static Random rnd = new Random();

        // Generate a random string
        public static String GetRandomName(int length = 5)
        {
            string characters = "0123456789abcdefghijklmnopqrstuvwxyz";
          
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[rnd.Next(characters.Length)]);
            }
            return result.ToString();
        }

        internal static Statement GenerateStatement(string resource)
        {
            Statement stmt = new Statement();
            stmt.resources = new Resources(resource);
            return stmt;
        }

        internal static string GenerateResourcesPrefix(string bucketName, string objectName)
        {
            return PolicyConstants.AWS_RESOURCE_PREFIX + bucketName + "/" + objectName;
        }

        internal static Statement GenerateStatement(List<string> actions,string resourcePrefix, string effect = "Allow", string aws = "*",bool withConditions=false)
        {
            Statement stmt = new Statement();
            stmt.resources = new Resources(resourcePrefix);
            stmt.actions = actions;
            stmt.effect = effect;
            stmt.principal = new Principal(aws);
            if (withConditions)
            {
                stmt.conditions = new ConditionMap();
                ConditionKeyMap ckmap = new ConditionKeyMap();
                ckmap.Add("s3:prefix", new HashSet<string>() { "hello" });
                stmt.conditions.Add("StringEquals", ckmap);
            }
            return stmt;
        }
    }
}
