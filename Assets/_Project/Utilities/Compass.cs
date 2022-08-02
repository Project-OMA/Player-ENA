using UnityEngine;

namespace ENA.Utilities
{
    public class Compass
    {
        #region Enums
        public enum Direction
        {
            Left, UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft, None
        }
        #endregion
        #region Variables
        [SerializeField] Transform referencePoint;
        [SerializeField] Direction direction;
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