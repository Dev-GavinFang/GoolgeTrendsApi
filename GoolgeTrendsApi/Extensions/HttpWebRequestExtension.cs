using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace System.Net
{
    static class HttpWebRequestExtension
    {
        public static async Task<string> GetTextResponseAsync(this HttpWebRequest request)
        {
            using (var response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
