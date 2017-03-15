using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Google.Cloud.Storage.V1;

namespace gcpstorage_benchmark
{
    class Program
    {
        private const string bucketName = "mybucket";
        private const string fileName = "folder/myfile.txt";
        private const int runCount = 50;

        static void Main(string[] args)
        {
            var client = StorageClient.Create();
            
            TestMethod(client);
            TestMethod(client);
            TestMethod(client);
            
            var measurements = new List<TimeSpan>();

            for(int i = 0; i < runCount; i++)
            {
                TestMethod(client, measurements);
            }
            
            foreach(var m in measurements)
            {
                System.Console.WriteLine("Duration: {0}ms", m.TotalMilliseconds);
            }
            
            System.Console.WriteLine("Average duration: {0}ms", measurements.Sum(m => m.TotalMilliseconds) / runCount);
        }
        
        static void TestMethod(StorageClient client, List<TimeSpan> measurements = null)
        {
            using(var ms = new MemoryStream())
            {
                var sw = Stopwatch.StartNew();
                client.DownloadObject(bucketName, fileName, ms);
                sw.Stop();
                
                if(measurements != null)
                {
                    measurements.Add(sw.Elapsed);
                }
            }
        }
    }
}
