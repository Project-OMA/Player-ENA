using System;
using UnityEngine;

namespace ENA.Provenance
{
    [Serializable]
    public partial class Results
    {
        #region Variables
        [SerializeField] bool clearedMap = false;
        [SerializeField] float totalSessionTime = 0;
        #endregion
        #region Constructors
        public Results(bool clearedMap, float totalSessionTime)
        {
            this.clearedMap = clearedMap;
            this.totalSessionTime = totalSessionTime;
        }
        #endregion
    }
}