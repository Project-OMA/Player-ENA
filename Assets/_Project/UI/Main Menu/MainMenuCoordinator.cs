using UnityEngine;

namespace ENA.UI
{
    public class MainMenuCoordinator: UICoordinator
    {
        #region Variables
        [SerializeField] GameObject shortcutObject;
        #endregion
        #region UICoordinator Implementation
        public override void Setup() {}
        #endregion
        #region Methods
        public void MoveToMapSection()
        {
            foreach(Transform child in shortcutObject.transform) {
                UAP_AccessibilityManager.SelectElement(child.gameObject);
                return;
            }
        }
        #endregion
    }
}