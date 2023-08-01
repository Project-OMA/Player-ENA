using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.UI
{
    public class AuthDisplay: UIPanel
    {
        #region References
        [SerializeField] TMP_InputField loginField;
        [SerializeField] TMP_InputField passwordField;
        #endregion
        #region Methods
        public (string login, string password) GetCredentials()
        {
            return (loginField.text, passwordField.text);
        }
        #endregion
    }
}