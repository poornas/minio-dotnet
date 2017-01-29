﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minio.DataModel;

namespace Minio.Examples.Cases
{
    class ListBuckets
    {
        public async static Task Run(Minio.MinioRestClient minio)
        {
            try
            {
                var bucketName = "bucket-name";
                var bucketObject = "bucket-object";

                bucketName = "mountshasta";
                IObservable<Upload> observable = minio.Objects.ListIncompleteUploads(bucketName, bucketObject, true);

                IDisposable subscription = observable.Subscribe(
                    item => Console.WriteLine("OnNext: {0}", item.Key),
                    ex => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnComplete: {0}"));

            }
            catch (Exception e)
            {
                Console.WriteLine("[Bucket]  Exception: {0}", e);
            }
        }

       
    }
}
