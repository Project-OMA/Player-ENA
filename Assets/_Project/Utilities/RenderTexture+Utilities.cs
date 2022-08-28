using System.IO;
using UnityEngine;

namespace ENA.Utilities
{
    public static partial class RenderTextureExtensions
    {
        public static void OutputToFile(this RenderTexture self, string destination)
        {
            Texture2D texture = self.ToTexture2D();
            File.WriteAllBytes(destination, texture.EncodeToPNG());
            Object.Destroy(texture);
        }

        public static Texture2D ToTexture2D(this RenderTexture self)
        {
            Texture2D tex = new Texture2D(self.width, self.height, TextureFormat.RGB24, false);
            RenderTexture.active = self;
            tex.ReadPixels(new Rect(0, 0, self.width, self.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}