using System;
using UnityEngine;
using ENA.Utilities;
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
            public static IDComparer New => new IDComparer();
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
        [SerializeField] uint mapID;
        [SerializeField] string folderPath;
        [SerializeField] string mapName;
        Sprite sprite;
        #endregion
        #region Properties
        public string FilePath => folderPath+MapFileName;
        public uint ID => mapID;
        public string Name => mapName;
        public Sprite Sprite {
            get {
                if (sprite == null) sprite = GenerateSprite();
                Debug.Log($"Sprite: {sprite}");
                return sprite;
            }
        }
        public string ThumbnailPath => folderPath+ImageFileName;
        #endregion
        #region Constructors
        public MapData(uint id, string mapName, string folderPath)
        {
            this.mapID = id;
            this.mapName = mapName;
            this.folderPath = folderPath;
        }
        #endregion
        #region Methods
        private Sprite GenerateSprite()
        {
            if (string.IsNullOrEmpty(folderPath)) return null;
            string imagePath = folderPath+ImageFileName;
            Debug.Log($"{imagePath}");

            Texture2D texture = new Texture2D(1,1);
            texture.LoadImage(imagePath);
            return texture.ToSprite();
        }
        #endregion
    }
}