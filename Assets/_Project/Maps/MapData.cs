using System;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Maps
{
    public class MapData
    {
        #region Constants
        const string MapFileName = "map.xml";
        const string ImageFileName = "thumbnail.png";
        #endregion
        #region Classes
        [Serializable]
        public class List
        {
            #region Variables
            public MapData[] maps;
            #endregion
        }
        #endregion
        #region Variables
        [SerializeField] string mapName;
        [SerializeField] string folderPath;
        Sprite sprite;
        #endregion
        #region Properties
        public Sprite Sprite => sprite;
        public string Name => mapName;
        public string FilePath => folderPath+MapFileName;
        #endregion
        #region Constructors
        public MapData(string mapName, string folderPath)
        {
            this.mapName = mapName;
            this.folderPath = folderPath;
            this.sprite = GenerateSprite();
        }
        #endregion
        #region Methods
        private Sprite GenerateSprite()
        {
            if (string.IsNullOrEmpty(folderPath)) return null;
            string imagePath = folderPath+ImageFileName;

            Texture2D texture = new Texture2D(1,1);
            texture.LoadImage(imagePath);
            return texture.ToSprite();
        }
        #endregion
    }
}