using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace spookyWebServer
{
    class Program
    {
        public static HttpListener http = new HttpListener();
        static List<duel> activeDuelChallenges = new List<duel>();
        static DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(duel));
        static int Main(string[] args)
        {
            http.Start();
            http.Prefixes.Add("http://localhost:8220/");
            while (true)
            {
                var context = http.GetContext();
                var request = context.Request;
                if (request.HttpMethod == "OPTIONS")
                {
                    options(context);
                }
                switch (request.Url.Segments[1])
                {
                    case "duel":
                        duel(context); break;
                }
            }
            return 0;
        }

        private static void options(HttpListenerContext context)
        {
            var response = context.Response;
            response.ContentLength64 = 0;

            byte[] test = { 0xfa };
            response.Headers.Add("Access-Control-Allow-Origin: *");
            response.Headers.Add("Access-Control-Allow-Methods: PUT, GET, POST, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers: Content-Type, origin");
            response.OutputStream.Write(test, 0, 0);
            response.OutputStream.Close();
        }

        private static void duel(HttpListenerContext context)
        {
            switch(context.Request.HttpMethod)
            {
                case "PUT":
                    activeDuelChallenges.Add((duel)json.ReadObject(context.Request.InputStream));
                    break;
                case "DELETE":
                    activeDuelChallenges.RemoveAll(x=>x.playerId==((duel)json.ReadObject(context.Request.InputStream)).playerId);
                    break;
                case "GET":
                    json.WriteObject(context.Response.OutputStream, activeDuelChallenges);
                    break;
            }
            context.Request.InputStream.Close();
        }
        
    }
    [DataContract]
    public struct duel
    {
        [DataMember]
        public int src;
        [DataMember]
        public int playerId;
        [DataMember]
        public int referee;
        [DataMember]
        public currency[] srcWager;
    }

    [DataContract]
    public struct currency
    {
        [DataMember]
        public currencies Type;
        [DataMember]
        public int count;
    }
    [DataContract]
    public struct currencies
    {
    }
}
