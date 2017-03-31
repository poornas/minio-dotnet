﻿/*
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
using System.Net;
using System.Net.Http;
using System.Collections.Generic;

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
            bool useAWS = true;
            if (useAWS && Environment.GetEnvironmentVariable("AWS_ENDPOINT") != null)
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
               // string smallFileName = CreateFile(1 * MB);
               // string bigFileName = CreateFile(6 * MB);
                string objectName = GetRandomName();
                string destBucketName = GetRandomName();
                string destObjectName = GetRandomName();
               
                // Set app Info 
                minioClient.SetAppInfo("app-name", "app-version");

                // Set HTTP Tracing On
                // minioClient.SetTraceOn();

                // Set HTTP Tracing Off
                // minioClient.SetTraceOff();
                /* TESTED SO FAR 
                // Check if bucket exists
                BucketExists_Test(minioClient).Wait();

                // Create a new bucket
                 MakeBucket_Test1(minioClient).Wait();
                 MakeBucket_Test2(minioClient).Wait();
                 MakeBucket_Test3(minioClient).Wait();
                 MakeBucket_Test4(minioClient).Wait();

                //Test removal of bucket
                RemoveBucket_Test1(minioClient).Wait();

                //Test ListBuckets function
                ListBuckets_Test(minioClient).Wait();

                //Test Putobject function
                PutObject_Test1(minioClient).Wait();
                PutObject_Test2(minioClient).Wait();

                PutObject_Test3(minioClient).Wait();
                PutObject_Test4(minioClient).Wait();

                //Test StatObject function
                StatObject_Test1(minioClient).Wait();

                //Test GetObjectAsync function
                GetObject_Test1(minioClient).Wait();
                GetObject_Test2(minioClient).Wait();

                //Test File GetObject and PutObject functions

                FGetObject_Test1(minioClient).Wait();
                FPutObject_Test1(minioClient).Wait();
                FPutObject_Test2(minioClient).Wait();
                
                //Test ListObjectAsync function
                ListObjects_Test1(minioClient).Wait();
                ListObjects_Test2(minioClient).Wait();

                //Test RemoveObjectAsync function
                RemoveObject_Test1(minioClient).Wait();

                //Test CopyObjectAsync function
                CopyObject_Test1(minioClient).Wait();
                CopyObject_Test2(minioClient).Wait();
                CopyObject_Test3(minioClient).Wait();
                CopyObject_Test4(minioClient).Wait();
               
                //Test SetPolicyAsync function
                SetBucketPolicy_Test1(minioClient).Wait();

                //Test Presigned Get/Put operations
                PresignedGetObject_Test1(minioClient).Wait();
                PresignedPutObject_Test1(minioClient).Wait();

                END WORKING TESTS
                */
                // TODO: 
                PresignedPostPolicy_Test1(minioClient);
                //GetBucketPolicy_Test1(minioClient).Wait();
                /*
                           
         
                                // List the incomplete uploads
                                ListIncompleteUploads(minioClient, bucketName);

                                // Remove all the incomplete uploads
                                RemoveIncompleteUpload(minioClient, bucketName, objectName).Wait();

                              // Get the policy for given bucket
                                GetBucketPolicy(minioClient, bucketName).Wait();

              
                                // Get the presigned POST policy curl url
                                PresignedPostPolicy(minioClient).Wait();

        

                                  */
                Console.ReadLine();
            }
            catch (MinioException ex)
            {
                Console.Out.WriteLine(ex.Message);
            }

        }
        private async static Task BucketExists_Test(MinioClient minio)
        {
            Console.Out.WriteLine("Test: BucketExistsAsync");
            string bucketName = GetRandomName();
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
        }
        private async static Task MakeBucket_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test 1: MakeBucketAsync");
            string bucketName = GetRandomName(length: 60);
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
        }
        private async static Task MakeBucket_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test 2 : MakeBucketAsync");
            string bucketName = GetRandomName(length: 10) + ".withperiod";
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
        }
        private async static Task MakeBucket_Test3(MinioClient minio)
        {
            Console.Out.WriteLine("Test 3 : MakeBucketAsync with region");
            string bucketName = GetRandomName(length: 60);
            try
            {
                await minio.MakeBucketAsync(bucketName, location: "eu-central-1");
                bool found = await minio.BucketExistsAsync(bucketName);
                Assert.IsTrue(found);
                if (found)
                {
                    await minio.MakeBucketAsync(bucketName);
                    await minio.RemoveBucketAsync(bucketName);

                }
            }
            catch (MinioException ex)
            {
                Assert.AreEqual<string>(ex.message, "The requested bucket name is not available. The bucket namespace is shared by all users of the system. Please select a different name and try again.");
            }
         
        }
        private async static Task MakeBucket_Test4(MinioClient minio)
        {
            Console.Out.WriteLine("Test 4 : MakeBucketAsync with region");
            string bucketName = GetRandomName(length: 20) + ".withperiod" ;
            try
            {
                await minio.MakeBucketAsync(bucketName, location: "us-west-2");
                bool found = await minio.BucketExistsAsync(bucketName);
                Assert.IsTrue(found);
                if (found)
                {
                    await minio.RemoveBucketAsync(bucketName);

                }
            }
            catch (MinioException ex)
            {
                Assert.Fail();
            }
        }

        private async static Task RemoveBucket_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test: RemoveBucketAsync");
            string bucketName = GetRandomName(length: 60);
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
            await minio.RemoveBucketAsync(bucketName);
            found = await minio.BucketExistsAsync(bucketName);
            Assert.IsFalse(found);
            Console.Out.WriteLine("Test: RemoveBucketAsync succeeded");

        }
        private async static Task ListBuckets_Test(MinioClient minio)
        {
            try
            {
                Console.Out.WriteLine("Test: ListBucketsAsync");
                var list = await minio.ListBucketsAsync();
                foreach (Bucket bucket in list.Buckets)
                {
                    //Ignore
                    continue;
                }
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
        private async static Task Setup_Test(MinioClient minio, string bucketName)
        {
            await minio.MakeBucketAsync(bucketName);
            bool found = await minio.BucketExistsAsync(bucketName);
            Assert.IsTrue(found);
        }

        private async static Task TearDown(MinioClient minio, string bucketName)
        {
            await minio.RemoveBucketAsync(bucketName);
        }
        private async static Task PutObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: PutobjectAsync with stream");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(3 * MB);
            string contentType = "application/octet-stream";
            await Setup_Test(minio, bucketName);
            await PutObject_Tester(minio, bucketName, objectName, fileName, contentType);
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: PutobjectAsync with stream complete");

        }

        private async static Task PutObject_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test2: PutobjectAsync with large stream");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(8 * MB);
            string contentType = "application/octet-stream";
            await Setup_Test(minio, bucketName);
            await PutObject_Tester(minio, bucketName, objectName, fileName, contentType);
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test2: PutobjectAsync with stream complete");

        }

        private async static Task PutObject_Test3(MinioClient minio)
        {
            Console.Out.WriteLine("Test3: PutobjectAsync with  different content-type");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            string contentType = "custom-contenttype";
            await Setup_Test(minio, bucketName);
            await PutObject_Tester(minio, bucketName, objectName, fileName, contentType);
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test3: PutobjectAsync with different content-type complete");

        }
        private async static Task PutObject_Test4(MinioClient minio)
        {
            //Putobject call with incorrect size of stream. See if PutObjectAsync call resumes 
            Console.Out.WriteLine("Test4: PutobjectAsync resume upload");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            string contentType = "application/octet-stream";
            await Setup_Test(minio, bucketName);
            try
            {
                byte[] bs = File.ReadAllBytes(fileName);
                System.IO.MemoryStream filestream = new System.IO.MemoryStream(bs);
                long size = 4 * MB;
                long file_write_size = filestream.Length;

                await minio.PutObjectAsync(bucketName,
                                           objectName,
                                           filestream,
                                           size,
                                           contentType);
            } 
            catch (UnexpectedShortReadException ex)
            { 
                //PutObject failed as expected since the stream size is incorrect
                //default to actual stream size and complete the upload
                await PutObject_Tester(minio, bucketName, objectName, fileName, contentType);

            }
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test4: PutobjectAsync with different content-type complete");
        }
        private async static Task PutObject_Tester(MinioClient minio, string bucketName,string objectName,string fileName=null,string contentType="application/octet-stream",long size = 0)
        {
            try
            {
                byte[] bs = File.ReadAllBytes(fileName);
                System.IO.MemoryStream filestream = new System.IO.MemoryStream(bs);
           
                long file_write_size = filestream.Length;
                long file_read_size = 0 ;
                string tempFileName = "tempfiletosavestream";
                if (size == 0)
                    size = filestream.Length;
                if (filestream.Length < (5 * MB))
                {
                    Console.Out.WriteLine("Test1: PutobjectAsync: PutObjectAsync with Stream");
                }
                else
                {
                    Console.Out.WriteLine("Test1: PutobjectAsync: PutObjectAsync with Stream and MultiPartUpload");
                }
                await minio.PutObjectAsync(bucketName,
                                           objectName,
                                           filestream,
                                           size,
                                           contentType);

                await minio.GetObjectAsync(bucketName, objectName,
               (stream) =>
               {
                   var fileStream = File.Create(tempFileName);
                   stream.CopyTo(fileStream);
                   fileStream.Dispose();
                   FileInfo writtenInfo = new FileInfo(tempFileName);
                   file_read_size = writtenInfo.Length;

                   Assert.AreEqual(file_read_size, file_write_size);
                   File.Delete(tempFileName);
                });
                ObjectStat statObject = await minio.StatObjectAsync(bucketName, objectName);
                Assert.IsNotNull(statObject);
                Assert.AreEqual(statObject.ObjectName, objectName);
                Assert.AreEqual(statObject.Size, file_read_size);
                Assert.AreEqual(statObject.ContentType, contentType);

                await minio.RemoveObjectAsync(bucketName, objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
                Assert.Fail();
            }

        }

        private async static Task StatObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: StatObjectAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            string contentType = "gzip";
            await Setup_Test(minio, bucketName);
            try
            {
                byte[] bs = File.ReadAllBytes(fileName);
                System.IO.MemoryStream filestream = new System.IO.MemoryStream(bs);
                long file_write_size = filestream.Length;
                 
                await minio.PutObjectAsync(bucketName,
                                           objectName,
                                           filestream,
                                           filestream.Length,
                                           contentType);

             
                ObjectStat statObject = await minio.StatObjectAsync(bucketName, objectName);
                Assert.IsNotNull(statObject);
                Assert.AreEqual(statObject.ObjectName, objectName);
                Assert.AreEqual(statObject.Size, file_write_size);
                Assert.AreEqual(statObject.ContentType, contentType);

                await minio.RemoveObjectAsync(bucketName, objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
                Assert.Fail();
            }
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: StatObjectAsync Complete");
           

        }
        private async static Task CopyObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: CopyObjectsAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string destBucketName = GetRandomName(15);
            string destObjectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await Setup_Test(minio, destBucketName);

            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            await minio.CopyObjectAsync(bucketName, objectName, destBucketName, destObjectName);
            string outFileName = "outFileName";
  
            await minio.GetObjectAsync(destBucketName, destObjectName, outFileName);
            File.Delete(outFileName);
            await minio.RemoveObjectAsync(bucketName, objectName);
            await minio.RemoveObjectAsync(destBucketName, destObjectName);


            await TearDown(minio, bucketName);
            await TearDown(minio, destBucketName);

            File.Delete(fileName);
            Console.Out.WriteLine("Test1: CopyObjectsAsync Complete");
        }

        private async static Task CopyObject_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test2: CopyObjectsAsync");
            //Test CopyConditions where matching ETag is not found
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string destBucketName = GetRandomName(15);
            string destObjectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await Setup_Test(minio, destBucketName);

            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            CopyConditions conditions = new CopyConditions();
            conditions.SetMatchETag("TestETag");
            try
            {
                await minio.CopyObjectAsync(bucketName, objectName, destBucketName, destObjectName, conditions);

            }
            catch (MinioException ex)
            {
                Assert.AreEqual(ex.Message, "Minio API responded with message=At least one of the pre-conditions you specified did not hold");
            }
 
            await minio.RemoveObjectAsync(bucketName, objectName);


            await TearDown(minio, bucketName);
            await TearDown(minio, destBucketName);

            File.Delete(fileName);
            Console.Out.WriteLine("Test2: CopyObjectsAsync Complete");
        }
        private async static Task CopyObject_Test3(MinioClient minio)
        {
            Console.Out.WriteLine("Test3: CopyObjectsAsync");
            //Test CopyConditions where matching ETag is found
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string destBucketName = GetRandomName(15);
            string destObjectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await Setup_Test(minio, destBucketName);

            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);
            ObjectStat stats = await minio.StatObjectAsync(bucketName, objectName);

            CopyConditions conditions = new CopyConditions();
            conditions.SetMatchETag(stats.ETag);
            try
            {
                await minio.CopyObjectAsync(bucketName, objectName, destBucketName, destObjectName, conditions);

            }
            catch (MinioException ex)
            {
                Assert.Fail();
            }
           
            string outFileName = "outFileName";
            ObjectStat dstats = await minio.StatObjectAsync(destBucketName, destObjectName);
            Assert.IsNotNull(dstats);
            Assert.AreEqual(dstats.ETag, stats.ETag);
            Assert.AreEqual(dstats.ObjectName, destObjectName);
            await minio.GetObjectAsync(destBucketName, destObjectName, outFileName);
            File.Delete(outFileName);
          
            await minio.RemoveObjectAsync(bucketName, objectName);
            await minio.RemoveObjectAsync(destBucketName, destObjectName);


            await TearDown(minio, bucketName);
            await TearDown(minio, destBucketName);

            File.Delete(fileName);
            Console.Out.WriteLine("Test3: CopyObjectsAsync Complete");
        }
        private async static Task CopyObject_Test4(MinioClient minio)
        {
            //Test if objectName is defaulted to source objectName
            Console.Out.WriteLine("Test4: CopyObjectsAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string destBucketName = GetRandomName(15);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await Setup_Test(minio, destBucketName);

            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            CopyConditions conditions = new CopyConditions();
            conditions.SetMatchETag("TestETag");
            //omit dest bucket name.
            await minio.CopyObjectAsync(bucketName, objectName, destBucketName);
            string outFileName = "outFileName";

            await minio.GetObjectAsync(bucketName, objectName, outFileName);
            File.Delete(outFileName);
            ObjectStat stats = await minio.StatObjectAsync(destBucketName, objectName);
            Assert.IsNotNull(stats);
            Assert.AreEqual(stats.ObjectName, objectName);
            await minio.RemoveObjectAsync(bucketName, objectName);
            await minio.RemoveObjectAsync(destBucketName, objectName);


            await TearDown(minio, bucketName);
            await TearDown(minio, destBucketName);

            File.Delete(fileName);
            Console.Out.WriteLine("Test4: CopyObjectsAsync Complete");
        }
        private async static Task GetObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: GetObjectAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            string contentType = null;
            await Setup_Test(minio, bucketName);
            try
            {
                byte[] bs = File.ReadAllBytes(fileName);
                System.IO.MemoryStream filestream = new System.IO.MemoryStream(bs);
                long file_write_size = filestream.Length;
                string tempFileName = "tempFileName";
                long file_read_size = 0;
                await minio.PutObjectAsync(bucketName,
                                           objectName,
                                           filestream,
                                           filestream.Length,
                                           contentType);

                await minio.GetObjectAsync(bucketName, objectName,
              (stream) =>
              {
                  var fileStream = File.Create(tempFileName);
                  stream.CopyTo(fileStream);
                  fileStream.Dispose();
                  FileInfo writtenInfo = new FileInfo(tempFileName);
                  file_read_size = writtenInfo.Length;

                  Assert.AreEqual(file_read_size, file_write_size);
                  File.Delete(tempFileName);
              });

                await minio.RemoveObjectAsync(bucketName, objectName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
                Assert.Fail();
            }
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: GetObjectAsync Complete");
        }
        private async static Task GetObject_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test2: GetObjectAsync for non existent object");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = GetRandomName(10);
            await Setup_Test(minio, bucketName);
            try
            {
                await minio.GetObjectAsync(bucketName, objectName,fileName);

            }
            catch (ObjectNotFoundException ex)
            {
                Assert.AreEqual(ex.message,"Not found.");
            }

            await TearDown(minio, bucketName);
            Console.Out.WriteLine("Test2: GetObjectAsync Complete");
        }
        private async static Task FGetObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: GetObjectAsync for download to file");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            string outFileName = "outFileName";
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            await minio.GetObjectAsync(bucketName, objectName, outFileName);
            File.Delete(outFileName);
            await minio.RemoveObjectAsync(bucketName, objectName);
             
            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: GetObjectAsync Complete");
        }

        private async static Task FPutObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: PutObjectAsync for upload from large file - multipart");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(10 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: PutObjectAsync Complete");
        }

        private async static Task FPutObject_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test2: PutObjectAsync for upload from small file");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test2: PutObjectAsync Complete");
        }
        private async static Task ListObjects_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: ListObjectsAsync");
            string bucketName = GetRandomName(15);
            string prefix = "minix";
            string objectName1 = prefix + GetRandomName(10);
            string objectName2 = prefix + GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName1,
                                        fileName);
            await minio.PutObjectAsync(bucketName,
                                       objectName2,
                                       fileName);
            ListObjects_Test(minio, bucketName, prefix,2).Wait();
            System.Threading.Thread.Sleep(5000);
            Console.Out.WriteLine("removing objects");
            await minio.RemoveObjectAsync(bucketName, objectName1);
            await minio.RemoveObjectAsync(bucketName, objectName2);


            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: ListObjectsAsync Complete");
        }

        private async static Task ListObjects_Test2(MinioClient minio)
        {
            Console.Out.WriteLine("Test2: ListObjectsAsync on empty bucket");
            string bucketName = GetRandomName(15);
            await Setup_Test(minio, bucketName);
        
            ListObjects_Test(minio, bucketName, null, 0).Wait();
           
            await TearDown(minio, bucketName);
             Console.Out.WriteLine("Test2: ListObjectsAsync Complete");
        }
        private async static Task ListObjects_Test(MinioClient minio,string bucketName, string prefix,int numObjects, bool recursive = true)
        {
            int count = 0;
            try
            {
                IObservable<Item> observable = minio.ListObjectsAsync(bucketName, prefix, recursive);
                IDisposable subscription = observable.Subscribe(
                    item =>
                    {
                        Assert.IsTrue(item.Key.StartsWith(prefix));
                        count += 1;
                        Console.Out.WriteLine(item.Key + ":" + count.ToString());
                    },
                    ex => Console.WriteLine("OnError: {0}", ex),
                    () =>
                    {
                        Console.WriteLine("Listed all objects in bucket " + bucketName + "\n");
                        Assert.AreEqual(count, numObjects);

                    });

                //subscription.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
            }
        }

        private async static Task RemoveObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: RemoveObjectAsync for existing object");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);

            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: RemoveObjectAsync Complete");
        }

     
        private async static Task PresignedGetObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: PresignedGetObjectAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            string downloadFile = "downloadFileName";
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);
            ObjectStat stats = await minio.StatObjectAsync(bucketName, objectName);
            string presigned_url = await minio.PresignedGetObjectAsync(bucketName, objectName, 1000);
            WebRequest httpRequest = WebRequest.Create(presigned_url);
            var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(httpRequest.BeginGetResponse, httpRequest.EndGetResponse, null));
            Stream stream = response.GetResponseStream();
            var fileStream = File.Create(downloadFile);
            stream.CopyTo(fileStream);
            fileStream.Dispose();
            FileInfo writtenInfo = new FileInfo(downloadFile);
            long file_read_size = writtenInfo.Length;
            //Compare size of file downloaded  with presigned curl request and actual object size on server
            Assert.AreEqual(file_read_size, stats.Size);

            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            File.Delete(downloadFile);
            Console.Out.WriteLine("Test1: PresignedGetObjectAsync Complete");
        }

        private async static Task PresignedPutObject_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: PresignedPutObjectAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            //Upload with presigned url
            string presigned_url = await minio.PresignedPutObjectAsync(bucketName, objectName, 1000);
            await UploadObjectAsync(presigned_url, fileName);
            //Get stats for object from server
            ObjectStat stats = await minio.StatObjectAsync(bucketName, objectName);
            //Compare with file used for upload
            FileInfo writtenInfo = new FileInfo(fileName);
            long file_written_size = writtenInfo.Length;
            Assert.AreEqual(file_written_size, stats.Size);

            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: PresignedPutObjectAsync Complete");
        }
        private static async Task UploadObjectAsync(string url,string filePath)
        {
            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "PUT";
            using (var dataStream = await Task.Factory.FromAsync<Stream>(httpRequest.BeginGetRequestStream, httpRequest.EndGetRequestStream, null))
            {
                byte[] buffer = new byte[8000];
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(httpRequest.BeginGetResponse, httpRequest.EndGetResponse, null));



        }
        private async static Task PresignedPostPolicy_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: PresignedPostPolicyAsync");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);


            try
            {
                await Setup_Test(minio, bucketName);
                await minio.PutObjectAsync(bucketName,
                            objectName,
                            fileName);
           
                // Generate presigned post policy url
                PostPolicy form = new PostPolicy();
                DateTime expiration = DateTime.UtcNow;
                form.SetExpires(expiration.AddDays(10));
                form.SetKey(objectName);
                form.SetBucket(bucketName);
                var pairs = new List<KeyValuePair<string, string>>();
                string url = "https://s3.amazonaws.com/" + bucketName  ;
                Tuple<string, System.Collections.Generic.Dictionary<string, string>> policyTuple = await minio.PresignedPostPolicyAsync(form);
                var httpClient = new HttpClient();

                using (var stream = File.OpenRead(fileName))
                 {
                    MultipartFormDataContent multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StreamContent(stream), fileName,objectName);
                    multipartContent.Add(new FormUrlEncodedContent(pairs));
                    var response = await httpClient.PostAsync(url, multipartContent );
                    response.EnsureSuccessStatusCode();
                }
                
                // Validate
                PolicyType policy = await minio.GetPolicyAsync(bucketName, objectName.Substring(5));
                Assert.AreEqual(policy.GetType(), PolicyType.READ_ONLY);
                await minio.RemoveObjectAsync(bucketName, objectName);

                await TearDown(minio, bucketName);
                File.Delete(fileName);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Exception ", e.Message);
            }
         
            Console.Out.WriteLine("Test1: PresignedPostPolicyAsync Complete");

   
            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: PresignedPostPolicyAsync Complete");
           
        }
        private async static Task RemoveIncompleteUpload(MinioClient minio, string bucketName,string objectName)
        {

        }
       
        // Set a policy for given bucket
        private async static Task SetBucketPolicy_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: SetPolicyAsync ");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);
            await minio.SetPolicyAsync(bucketName,
                                 objectName.Substring(5),
                                 PolicyType.READ_ONLY);
            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: SetPolicyAsync Complete");

        }

        // Set a policy for given bucket
        private async static Task GetBucketPolicy_Test1(MinioClient minio)
        {
            Console.Out.WriteLine("Test1: GetPolicyAsync ");
            string bucketName = GetRandomName(15);
            string objectName = GetRandomName(10);
            string fileName = CreateFile(1 * MB);
            await Setup_Test(minio, bucketName);
            await minio.PutObjectAsync(bucketName,
                                        objectName,
                                        fileName);
            await minio.SetPolicyAsync(bucketName,
                                 objectName.Substring(5),
                                 PolicyType.READ_ONLY);
            PolicyType policy = await minio.GetPolicyAsync(bucketName, objectName.Substring(5));
            Assert.AreEqual(policy.GetType(), PolicyType.READ_ONLY);
            await minio.RemoveObjectAsync(bucketName, objectName);

            await TearDown(minio, bucketName);
            File.Delete(fileName);
            Console.Out.WriteLine("Test1: GetPolicyAsync Complete");

        }

    }
}
