using System.Collections.Generic;
using UnityEngine;

namespace ENA.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel: MonoBehaviour
    {
        #region Variables
        CanvasGroup canvasGroup;
        #endregion
        #region Methods
        void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Start()
        {
            if (canvasGroup == null) Reset();
        }
        #endregion
    }
}