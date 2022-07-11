using TMPro;
using UnityEngine;

namespace ENA.UI
{
    public class MainMenuDisplay: UIPanel
    {
        #region Variables
        [SerializeField] TextMeshProUGUI userLabel;
        [SerializeField] UIList mapDataListRoot;
        #endregion
        #region Methods
        public void SetHeader(string text)
        {
            userLabel.text = text;
        }

        public UIList GetList()
        {
            return mapDataListRoot;
        }
        #endregion
    }
}