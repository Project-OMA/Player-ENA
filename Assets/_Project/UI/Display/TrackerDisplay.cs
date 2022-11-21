using TMPro;
using UnityEngine;

namespace ENA.UI
{
    public class TrackerDisplay: UIPanel
    {
        #region Variables
        [SerializeField] TextMeshPro annotationLabel;
        #endregion
        #region Methods
        public void SetAnnotation(string text)
        {
            annotationLabel.text = text;
        }
        #endregion
    }
}