using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace spookyWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            http.Start();
            var context=http.GetContext();
            switch (context.Request.HttpMethod)
            {
                case "PUT":
                    duel(context.Request.InputStream);
                    break;
            }
        }

        private static void duel(Stream inputStream)
        {
        }
        public static HttpListener http=new HttpListener();
    }
}
