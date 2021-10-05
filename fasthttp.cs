using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace httprequestsender {
    class Program {
        static int numThreadsDone = 0;

        private static async Task DoShit(int numReq) {
            Console.WriteLine("Thread started : " + Thread.GetCurrentProcessorId().ToString());
            string proxy = "182.54.239.1:8018";
            NetworkCredential credentials = new NetworkCredential("ucpnrafu", "jdl9i4rgw5x9");

            HttpClient client;
            WebProxy webProxy = new WebProxy("http://" + proxy, false);
            bool useCredentials = true;
            if (useCredentials) {
                webProxy.Credentials = credentials;
            }
            HttpClientHandler handler = new HttpClientHandler {
                Proxy = webProxy,
                UseProxy = true
            };
            client = new HttpClient(handler);
            /*HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "");
            request.Headers.Add("Authorization", "");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36");
            request.Headers.Add("X-Ratelimit-Precision", "millisecond");*/
            for (int i = 0; i < numReq; i++) {
                HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Put, "https://jsonblob.com/api/jsonBlob/895035432572829696");
                request2.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36");
                request2.Content = new StringContent("{\"foo\": \"bar\"}", Encoding.UTF8, "application/json");

                HttpResponseMessage r = await client.SendAsync(request2);
                
            }
            numThreadsDone++;
            Console.WriteLine("Completed " + numReq.ToString() + " requests (" + numThreadsDone.ToString() + "/8)");
        }

        static async Task Thing() {
            var watch = new System.Diagnostics.Stopwatch();
            int items = 10000;
            int itemsPerThread = (int)(items / 8);
            watch.Start();
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 8; i++) {
                Thread thread = new Thread(async () => await DoShit(itemsPerThread));
                thread.Start();
                threads.Add(thread);
                
            }
            while (numThreadsDone < 8) {
                Thread.Sleep(10);
            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms to send " + items.ToString() + " requests: ~" + (((float)items/watch.ElapsedMilliseconds)*1000).ToString() + " requests / s");
        }

        static void Main(string[] args) {
            Thing();
            
            int delay = 100000;
            Thread.Sleep(delay);
        }
    }
}
