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
using System.Text.RegularExpressions;
namespace Minio
{
    public class Regions
    {
        private Regions()
        {
        }

        /// <summary>
        /// Get corresponding region for input host.
        /// </summary>
        /// <param name="endpoint">S3 API endpoint</param>
        /// <returns>Region corresponding to the endpoint. Default is 'us-east-1'</returns>
        public static string GetRegionFromEndpoint(string endpoint)
        {
            string region = null;
            Regex rgx = new Regex("^([a-z0-9][a-z0-9\\.\\-]{1,61}[a-z0-9])*?.?s3[.-]?(.*?)\\.amazonaws\\.com$", RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(endpoint);
            Console.Out.WriteLine("matches ...."+ matches + ".." + endpoint);
            Console.Out.WriteLine("groupsd././.", matches.Count , " endpoint...", endpoint);
            if ((matches.Count > 0) && (matches[0].Groups.Count> 1))
            {
                region = matches[0].Groups[2].Value;
            }
            return (region == null) ? "" : region;
        }
       
    }
}
