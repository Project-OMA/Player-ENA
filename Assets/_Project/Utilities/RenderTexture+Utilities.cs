using UnityEngine;

namespace JurassicEngine
{
    public static partial class RenderTextureExtensions
    {
        public static Texture2D ToTexture2D(this RenderTexture self)
        {
            Texture2D texture = new Texture2D(self.width, self.height, TextureFormat.RGB24, false);
            RenderTexture.active = self;
            texture.ReadPixels(new Rect(0, 0, self.width, self.height), 0, 0);
            texture.Apply();
            return texture;
        }
    }
}