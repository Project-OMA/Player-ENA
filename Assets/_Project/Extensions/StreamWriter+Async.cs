using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ENA
{
    public static partial class StreamWriterExtensions
    {
        public static async Task WriteAsync(this StreamWriter self, string content)
        {
            await self.WriteAsync(content);
        }

        public static async Task WriteLinesAsync(this StreamWriter self, params string[] lines)
        {
            foreach(var line in lines) {
                await self.WriteLineAsync(line);
            }
        }
    }
}