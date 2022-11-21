using UnityEngine;

namespace ENA.UI
{
    public abstract class UICoordinator: MonoBehaviour
    {
        #region Variables
        protected UIManager manager;
        #endregion
        #region Abstract Implementation
        public abstract void Setup();
        #endregion
        #region Methods
        public void Setup(UIManager manager)
        {
            this.manager = manager;
            Setup();
        }
        #endregion
    }
}