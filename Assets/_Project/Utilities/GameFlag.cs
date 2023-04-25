using UnityEngine;

namespace ENA.Utilities
{
    [CreateAssetMenu(fileName="Game Flag", menuName="ENA/Game Flag")]
    public class GameFlag: ScriptableObject
    {
        #region Properties
        [field: SerializeField] public bool Value {get; private set;}
        #endregion
        #region Events
        public Event<bool> OnChangeValue;
        #endregion
        #region Methods
        public void Set(bool value)
        {
            Value = value;
            OnChangeValue.Invoke(value);
        }
        #endregion
    }
}