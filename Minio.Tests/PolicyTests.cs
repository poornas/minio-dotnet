using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.DataModel;
using Minio.DataModel.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Minio.Tests
{
    [TestClass]
    public class PolicyTests
    {
        /*
        [TestMethod]
        public void TestIfConditionMapGetsSerialized()
        {

            ConditionMap cmap = new ConditionMap();
            ConditionKeyMap ckmap = new ConditionKeyMap("s3:x-amz-acl", "public-read");
            cmap.Add("StringEquals", ckmap);
            string serialized_cmap = JsonConvert.SerializeObject(cmap);
            Assert.IsNotNull(serialized_cmap);
        }
        */
        [TestMethod]
        public void TestIfConditionMapGetsDeSerialized_Test1()
        {
            string conditionString = @"{""StringEquals"":{""s3: prefix"":""dotnetcms1ssazhd""}}";
            ConditionMap cmap = JsonConvert.DeserializeObject<ConditionMap>(conditionString, new ConditionMapConverter());
        }

        [TestMethod]
        public void TestIfConditionMapGetsDeSerialized_Test2()
        {
            string conditionString = @"{""StringEquals"":{""s3:x-amz-acl"":[""hello""]}}";
            ConditionMap cmap = JsonConvert.DeserializeObject<ConditionMap>(conditionString, new ConditionMapConverter());


        }
       
        [TestMethod]
        public void TestIfBucketPolicyGetsDeSerialized_Test1()
        {
            string policyString = @"{""Version"":""2012 - 10 - 17"",""Statement"":[{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: GetBucketLocation"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt""},{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: ListBucket"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt"",""Condition"":{""StringEquals"":{""s3: prefix"":""dotnetcms1ssazhd""}}},{""Sid"":"""",""Effect"":""Allow"",""Principal"":{""AWS"":"" * ""},""Action"":""s3: GetObject"",""Resource"":""arn: aws: s3:::miniodotnetvpn5pic718xfutt / dotnetcms1ssazhd * ""}]}";
            var contentBytes = System.Text.Encoding.UTF8.GetBytes(policyString);
            string bucketName = "miniodotnetvpn5pic718xfutt";
            var stream = new MemoryStream(contentBytes);
            BucketPolicy policy = BucketPolicy.parseJson(stream, bucketName);
        }
         
    }
}
