using System.IO;
using UnityEngine;

namespace ENA
{
    public static partial class Texture2DExtensions
    {
        public static Texture2D LoadImage(this Texture2D self, string filePath)
        {
            byte[] fileData;

            if (File.Exists(filePath)) {
                fileData = File.ReadAllBytes(filePath);
                self.LoadImage(fileData);
            }
            return self;
        }

        public static Sprite ToSprite(this Texture2D self, float pixelsPerUnit = 100f, SpriteMeshType meshType = SpriteMeshType.Tight)
        {
            return Sprite.Create(self, new Rect(0, 0, self.width, self.height), Vector2.zero, pixelsPerUnit, 0, meshType);
        }
    }
}