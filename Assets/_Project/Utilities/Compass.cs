using UnityEngine;
using ENA.Physics;

namespace ENA.Utilities
{
    public class Compass
    {
        #region Variables
        [SerializeField] Transform referencePoint;
        [SerializeField] Direction.Basic direction;
        #endregion
        #region Methods
        public void DefineDirection()
        {
            var position = referencePoint.position;
            throw new System.NotImplementedException();
        }

        void Update()
        {
            DefineDirection();
        }
        #endregion
    }
}