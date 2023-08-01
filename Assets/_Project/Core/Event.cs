using System;
using UnityEngine;
using UnityEngine.Events;

namespace ENA
{
    [Serializable]
    public struct Event
    {
        #region Variables
        [SerializeField] UnityEvent unityEvent;
        [SerializeField] Action action;
        #endregion
        #region Methods
        public void Invoke()
        {
            unityEvent?.Invoke();
            action?.Invoke();
        }
        #endregion
        #region Operators
        public static Event operator +(Event lhs, Action rhs)
        {
            lhs.action += rhs;
            return lhs;
        }

        public static Event operator -(Event lhs, Action rhs)
        {
            lhs.action -= rhs;
            return lhs;
        }
        #endregion
    }
    [Serializable]
    public struct Event<T>
    {
        #region Variables
        [SerializeField] UnityEvent<T> unityEvent;
        [SerializeField] Action<T> action;
        #endregion
        #region Methods
        public void Invoke(T value)
        {
            unityEvent?.Invoke(value);
            action?.Invoke(value);
        }
        #endregion
        #region Operators
        public static Event<T> operator +(Event<T> lhs, Action<T> rhs)
        {
            lhs.action += rhs;
            return lhs;
        }

        public static Event<T> operator -(Event<T> lhs, Action<T> rhs)
        {
            lhs.action -= rhs;
            return lhs;
        }
        #endregion
    }
}