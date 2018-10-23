using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace spookyWebServer
{
    public static class json
    {
        public static void write<T>(Stream stream, T obj)
        {
            var writer = new DataContractJsonSerializer(typeof(T));
            writer.WriteObject(stream, obj);
        }
        public static T read<T>(Stream stream)
        {
            var reader = new DataContractJsonSerializer(typeof(T));
            return (T)reader.ReadObject(stream);
        }
    }
}
