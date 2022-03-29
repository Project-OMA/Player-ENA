using TMPro;
using UnityEngine;

namespace ENA.UI {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILabel: MonoBehaviour
    {
        #region Variables
        [SerializeField] TextMeshProUGUI textLabel;
        #endregion
        #region Methods
        void Start()
        {
            Debug.Log($"Língua em uso: {Application.systemLanguage}");
        }

        public void SetText(string text)
        {
            textLabel.text = text;
        }
        #endregion
    }
}