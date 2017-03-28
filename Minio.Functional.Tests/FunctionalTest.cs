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
using Minio.Exceptions;
using System.Text;
using System.IO;
using Minio.DataModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Minio.Functional.Tests

{
    class FunctionalTest
    {
        private static Random rnd = new Random();
        private static int MB = 1024 * 1024;

        // Create a file of given size from random byte array
        private static String CreateFile(int size)
        {
            String fileName = GetRandomName();
            byte[] data = new byte[size];
            rnd.NextBytes(data);

            File.WriteAllBytes(fileName, data);

            return fileName;
        }

        // Generate a random string
        public static String GetRandomName(int length=5)
        {
            string characters = "0123456789abcdefghijklmnopqrstuvwxyz";
            if (length > 50)
                length = 50;
            StringBuilder result = new StringBuilder(length);
           
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[rnd.Next(characters.Length)]);
            }
            return "miniodotnet" + result.ToString();
        }

        public static void Main(string[] args)
        {
            String endPoint = null;
            String accessKey = null;
            String secretKey = null;
           
            if (Environment.GetEnvironmentVariable("AWS_ENDPOINT") != null)
            {
                endPoint = Environment.GetEnvironmentVariable("AWS_ENDPOINT");
                accessKey = Environment.GetEnvironmentVariable("MY_AWS_ACCESS_KEY");
                secretKey = Environment.GetEnvironmentVariable("MY_AWS_SECRET_KEY");
            }
            else
            {
                endPoint = "play.minio.io:9000";
                accessKey = "Q3AM3UQ867SPQQA43P2F";
                secretKey = "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG";
            }

            // WithSSL() enables SSL support in Minio client
            var minioClient = new Minio.MinioClient(endPoint, accessKey, secretKey).WithSSL();

            try
            {
                // Assign parameters before starting the test 
                string bucketName = GetRandomName();
                string smallFileName = CreateFile(1 * MB);
                string bigFileName = CreateFile(6 * MB);
                string objectName = GetRandomName();
                string destBucketName = GetRandomName();
                string destObjectName = GetRandomName();

                // Set app Info 
                minioClient.SetAppInfo("app-name", "app-version");

                // Set HTTP Tracing On
                minioClient.SetTraceOn();

                // Set HTTP Tracing Off
                // minioClient.SetTraceOff();

                // Check if bucket exists
                //BucketExists(minioClient, bucketName).Wait();

                // Create a new bucket
                MakeBucket(minioClient).Wait();

                // List all the buckets on the server
                ListBuckets(minioClient).Wait();

                // Put an object to the new bucket
                PutObject(minioClient, bucketName, objectName, smallFileName).Wait();

                // Get object metadata
                StatObject(minioClient, bucketName, objectName).Wait();

                // List the objects in the new bucket
                ListObjects(minioClient, bucketName);

                // Delete the file and Download the object as file
                GetObject(minioClient, bucketName, objectName, smallFileName).Wait();

                // Server side copyObject
                CopyObject(minioClient, bucketName, objectName, destBucketName, objectName).Wait();

                // Upload a File with PutObject
                FPutObject(minioClient, bucketName, objectName, smallFileName).Wait();

                // Delete the file and Download the object as file
                FGetObject(minioClient, bucketName, objectName, smallFileName).Wait();

                // Automatic Multipart Upload with object more than 5Mb
                PutObject(minioClient, bucketName, objectName, bigFileName).Wait();

                // List the incomplete uploads
                ListIncompleteUploads(minioClient, bucketName);

                // Remove all the incomplete uploads
                RemoveIncompleteUpload(minioClient, bucketName, objectName).Wait();

                // Set a policy for given bucket
                SetBucketPolicy(minioClient, PolicyType.READ_ONLY, bucketName).Wait();

                // Get the policy for given bucket
                GetBucketPolicy(minioClient, bucketName).Wait();

                // Get the presigned url for a GET object request
                PresignedGetObject(minioClient, bucketName, objectName).Wait();

                // Get the presigned POST policy curl url
                PresignedPostPolicy(minioClient).Wait();

                // Get the presigned url for a PUT object request
                PresignedPutObject(minioClient, bucketName, objectName).Wait();


                // Delete the object
                RemoveObject(minioClient, bucketName, objectName).Wait();

                // Delete the object
                RemoveObject(minioClient, destBucketName, objectName).Wait();

                // Remove the buckets
                RemoveBucket(minioClient, bucketName).Wait();
                RemoveBucket(minioClient, destBucketName).Wait();

                // Remove the binary files created for test
                File.Delete(smallFileName);
                File.Delete(bigFileName);

                Console.ReadLine();
            }
            catch (MinioException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }

        }
        private async static Task BucketExists(MinioClient minio, string bucketName)
        {
            Console.Out.WriteLine("Test: BucketExistsAsync");
            string name = GetRandomName();
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
        }
        private async static Task MakeBucket(MinioClient minio, string bucketName=null)
        {
            Console.Out.WriteLine("Test: MakeBucketAsync");
            if (bucketName == null)
            {
                bucketName = GetRandomName(length: 60);
            }
            await minio.MakeBucketAsync(bucketName);
            try
            {
                await minio.MakeBucketAsync("mountshasta");

            }
            catch (MinioException ex)
            {
                Assert.AreEqual<string>(ex.message, "The requested bucket name is not available. The bucket namespace is shared by all users of the system. Please select a different name and try again.");
            }
            await minio.RemoveBucketAsync(bucketName);
     
            bucketName = GetRandomName(length: 50);
            try
            {
                //TODO: Failing test
               // await minio.MakeBucketAsync(bucketName, "us-west-1");
               // await minio.RemoveBucketAsync(bucketName);

            }
            catch (AggregateException aggEx)
            {
                aggEx.Handle(HandleBatchExceptions);
            }
         
        }

        private static bool HandleBatchExceptions(Exception exceptionToHandle)
        {
            if (exceptionToHandle is ArgumentNullException)
            {
                //I'm handling the ArgumentNullException.
                Console.Out.WriteLine("Handling the ArgumentNullException.");
                //I handled this Exception, return true.
                return true;
            }
            else
            {
                //I'm only handling ArgumentNullExceptions.
                Console.Out.WriteLine(string.Format("I'm not handling the {0}.", exceptionToHandle.GetType()));
                //I didn't handle this Exception, return false.
                return false;
            }
        }

        private async static Task RemoveBucket(MinioClient minio,string bucketName)
        {
            Console.Out.WriteLine("Test: RemoveBucketAsync");
            string name = GetRandomName();
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
            found = await minio.BucketExistsAsync(bucketName);
            Assert.IsFalse(found);
        }
        private async static Task ListBuckets(MinioClient minio)
        {

        }
        private async static Task PutObject(MinioClient minio, string bucketName,string objectName, string fileName=null)
        {
        }
        private async static Task CopyObject(MinioClient minio, string bucketName, string objectName, string fdestBucketName, string destObjectName)
        {
        }
        private async static Task StatObject(MinioClient minio, string bucketName, string objectName)
        {
        }
        private async static Task GetObject(MinioClient minio, string bucketName, string objectName, string fileName = null)
        {
        }
        private async static Task ListObjects(MinioClient minio, string bucketName)
        {
        }
        private async static Task FGetObject(MinioClient minio, string bucketName, string objectName, string fileName = null)
        {
        }
        private async static Task FPutObject(MinioClient minio, string bucketName, string objectName, string fileName = null)
        {
        }
        private async static Task ListIncompleteUploads(MinioClient minio, string bucketName)
        {
        }

        private async static Task RemoveIncompleteUpload(MinioClient minio, string bucketName,string objectName)
        {

        }
        private async static Task RemoveObject(MinioClient minio, string bucketName, string objectName)
        {

        }
        // Set a policy for given bucket
        private async static Task SetBucketPolicy(MinioClient minio, PolicyType policy,string  bucketName)
        {

        }
        private async static Task GetBucketPolicy(MinioClient minio,  string bucketName)
        {

        }
        private async static Task PresignedGetObject(MinioClient minio, string bucketName,string objectName)
        {

        }
        private async static Task PresignedPostPolicy(MinioClient minio)
        {

        }
        private async static Task PresignedPutObject(MinioClient minio, string bucketName, string objectName)
        {

        }

        /// <summary>
        /// Task that uploads a file to a bucket
        /// </summary>
        /// <param name="minio"></param>
        /// <returns></returns>
        private async static Task Run(MinioClient minio)
        {
            // Make a new bucket called mymusic.
            var bucketName = "mymusic-folder"; //<==== change this
            var location = "us-east-1";
            // Upload the zip file
            var objectName = "my-golden-oldies.mp3";
            var filePath = "C:\\Users\\vagrant\\Downloads\\golden_oldies.mp3";
            var contentType = "application/zip";

            try
            {
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, location);
                }
                await minio.PutObjectAsync(bucketName, objectName, filePath, contentType);
                Console.Out.WriteLine("Successfully uploaded " + objectName);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }
        }
    }
}
