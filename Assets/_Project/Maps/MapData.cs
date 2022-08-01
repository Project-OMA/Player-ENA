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
        [SerializeField] string folderPath;
        [SerializeField] string mapName;
        Sprite sprite;
        #endregion
        #region Properties
        public string FolderPath => folderPath+MapFileName;
        public string Name => mapName;
        public Sprite Sprite => sprite;
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
            Debug.Log($"{imagePath}");

            Texture2D texture = new Texture2D(1,1);
            texture.LoadImage(imagePath);
            return texture.ToSprite();
        }
        #endregion
    }
}