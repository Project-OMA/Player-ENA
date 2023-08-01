using UnityEngine;

namespace ENA.Player
{
    public partial class RotationComponent: ExtendedMonoBehaviour
    {
        #region Properties
        [field: SerializeField] public float RotationSpeed {get; private set;}
        [field: SerializeField] public float SnapAngle {get; private set;}
        #endregion
        #region Methods
        public void RotateTowards(Quaternion target) => RotateTowards(target, RotationSpeed * Time.deltaTime);
        public void RotateTowards(Quaternion target, float maxDegrees) => Transform.RotateTowards(target, Mathf.Max(SnapAngle, maxDegrees));
        #endregion
    }
}