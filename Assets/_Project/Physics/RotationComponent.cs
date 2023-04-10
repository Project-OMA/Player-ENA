using UnityEngine;

namespace ENA.Physics
{
    public partial class RotationComponent: ExtendedMonoBehaviour
    {
        #region Variables
        #endregion
        #region Properties
        [field: SerializeField] public float RotationSpeed {get; private set;}
        [field: SerializeField] public float SnapAngle {get; private set;}
        #endregion
        #region Static Properties
        #endregion
        #region Events
        #endregion
        #region MonoBehaviour Lifecycle
        #endregion
        #region Methods
        public void RotateTowards(Quaternion target)
        {
            RotateTowards(target, RotationSpeed * Time.deltaTime);
        }

        public void RotateTowards(Quaternion target, float maxDegrees)
        {
            Transform.RotateTowards(target, Mathf.Max(SnapAngle, maxDegrees));
        }
        #endregion
        #region Operators
        #endregion
        #region Static Methods
        #endregion
    }
}