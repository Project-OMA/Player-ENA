using ENA.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    public class MapInfoDisplay: UIPanel
    {
        #region Variables
        [Header("References")]
        [SerializeField] Image mapThumbnail;
        [SerializeField] TextMeshProUGUI titleLabel;
        #endregion
        #region Methods
        public void SetMapData(MapData data)
        {
            mapThumbnail.sprite = data.Sprite;
            titleLabel.text = data.Name;
        }
        #endregion
    }
}