using System.Collections.Generic;
using UnityEngine;

namespace ENA.UI
{
    public partial class UIManager: MonoBehaviour
    {
        #region Variables
        [SerializeField] UICoordinator rootCoordinator;
        List<UIPanel> viewStack = new List<UIPanel>();
        #endregion
        #region Methods
        void Awake()
        {
            rootCoordinator.Setup(this);
        }

        public void Push(UIPanel panel)
        {
            if (viewStack.Count > 1) {
                var lastView = viewStack[viewStack.Count-1];
                lastView.gameObject.SetActive(false);
            }

            viewStack.Add(panel);
            panel.gameObject.SetActive(true);
        }

        public void Pop(UIPanel panel)
        {
            viewStack.Remove(panel);
            panel.gameObject.SetActive(false);

            if (viewStack.Count > 0) {
                var lastView = viewStack[viewStack.Count-1];
                lastView.gameObject.SetActive(true);
            }
        }

        public void Reset()
        {
            foreach(var panel in viewStack) {
                panel.gameObject.SetActive(false);
            }
            viewStack.Clear();
        }
        #endregion
    }
}