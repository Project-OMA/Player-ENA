using System;
using ENA.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    public class MapDataDisplay: UIPanel
    {
        #region Variables
        [SerializeField] Button button;
        [SerializeField] Image thumbnailImage;
        [SerializeField] TextMeshProUGUI titleLabel;
        #endregion
        #region Methods
        public void SetMapData(MapData mapData)
        {
            thumbnailImage.sprite = mapData.Sprite;
            titleLabel.text = mapData.Name;
            button.onClick.RemoveAllListeners();
        }

        public void SetBind(Action<int> action)
        {
            if (action == null) return;

            int siblingIndex = transform.GetSiblingIndex();
            button.onClick.AddListener(delegate{action(siblingIndex);});
        }
        #endregion
    }
}