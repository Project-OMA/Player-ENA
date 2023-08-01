using System;
using UnityEngine;
using System.Collections.Generic;

namespace ENA.Maps
{
    public class MapData
    {
        #region Constants
        public const string MapFileName = "map.xml";
        public const string ImageFileName = "thumbnail.png";
        public const string MetadataFileName = "ena.json";
        #endregion
        #region Classes
        public struct IDComparer: IEqualityComparer<MapData>
        {
            #region Static Variables
            public static IDComparer New => new();
            #endregion
            #region IEqualityComparer Implementation
            public bool Equals(MapData x, MapData y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(MapData obj)
            {
                return (int)obj.ID;
            }
            #endregion
        }
        #endregion
        #region Variables
        [SerializeField] string folderPath;
        [SerializeField] uint id;
        [SerializeField] string name;
        Sprite sprite;
        #endregion
        #region Properties
        public string FilePath => folderPath+MapFileName;
        public uint ID => id;
        public string Name => name;
        public Sprite Sprite {
            get {
                if (sprite == null) sprite = GenerateSprite();
                #if ENABLE_LOG
                Debug.Log($"Sprite: {sprite}");
                #endif
                return sprite;
            }
        }
        public string ThumbnailPath => folderPath+ImageFileName;
        #endregion
        #region Constructors
        public MapData(uint id, string mapName, string folderPath)
        {
            this.id = id;
            this.name = mapName;
            this.folderPath = folderPath;
        }
        #endregion
        #region Methods
        private Sprite GenerateSprite()
        {
            if (string.IsNullOrEmpty(folderPath)) return null;
            string imagePath = folderPath+ImageFileName;
            #if ENABLE_LOG
            Debug.Log($"{imagePath}");
            #endif

            Texture2D texture = new(1,1);
            texture.LoadImage(imagePath);
            return texture.ToSprite();
        }
        #endregion
    }
}