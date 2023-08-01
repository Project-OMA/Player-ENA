using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace ENA
{
    public static partial class DownloadHandlerExtensions
    {
        public static async Task<bool> WriteToFile<T>(this T self, string output, bool overrideOutput = false) where T: DownloadHandler
        {
            if (File.Exists(output) && !overrideOutput) return false;

            var fileInfo = new FileInfo(output);
            fileInfo.Directory.Create();

            using (FileStream stream = new(output, FileMode.OpenOrCreate)) {
                var data = self.data;
                await stream.WriteAsync(data, 0, data.Length);
            }

            return true;
        }

        public static async Task WhenComplete<T>(this T self, Action<T> onComplete) where T: DownloadHandler
        {
            do await Task.Delay(10);
            while (!self.isDone);

            onComplete?.Invoke(self);
        }

        public static DownloadHandlerAudioClip ToAudio(this DownloadHandler self)
        {
            if (self is DownloadHandlerAudioClip handler) {
                return handler;
            } else {
                return default;
            }
        }

        public static DownloadHandlerTexture ToImage(this DownloadHandler self)
        {
            if (self is DownloadHandlerTexture handler) {
                return handler;
            } else {
                return default;
            }
        }
    }
}