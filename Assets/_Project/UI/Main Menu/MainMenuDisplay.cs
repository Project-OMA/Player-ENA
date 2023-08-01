using TMPro;
using UnityEngine;

namespace ENA.UI
{
    public class MainMenuDisplay: UIPanel
    {
        #region Variables
        [SerializeField] TextMeshProUGUI userLabel;
        #endregion
        #region Methods
        public void SetHeader(string text)
        {
            userLabel.text = text;
        }
        #endregion
    }
}