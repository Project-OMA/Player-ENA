using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ENA.Utilities;
using UnityEngine;
using Event = ENA.Utilities.Event;

namespace ENA.Goals
{
    [CreateAssetMenu(fileName = "New List", menuName = "ENA/Objective List")]
    public class ObjectiveList: ScriptableObject, ICollection<ObjectiveComponent>
    {
        #region Variables
        [SerializeField] List<ObjectiveComponent> current = new List<ObjectiveComponent>();
        [SerializeField] List<ObjectiveComponent> cleared = new List<ObjectiveComponent>();
        #endregion
        #region Properties
        public int AmountCleared => cleared.Count;
        public int AmountLeft => current.Count;
        public bool ClearedAllObjectives => AmountLeft == 0;
        public int Count => AmountCleared + AmountLeft;
        public ObjectiveComponent NextObjective => current.FirstOrDefault();
        #endregion
        #region Events
        [Header("Events")]
        public Event<ObjectiveComponent> OnClearObjective;
        public Event OnClearAllObjectives;
        #endregion
        #region ScriptableObject Lifecycle
        #endregion
        #region Methods
        public void Check(ObjectiveComponent objective)
        {
            if (objective != NextObjective) return;

            Mark(objective);
        }

        private void Mark(ObjectiveComponent objective)
        {
            cleared.Add(objective);
            current.Remove(objective);
            OnClearObjective.Invoke(objective);

            if (AmountLeft == 0) OnClearAllObjectives.Invoke();
        }

        public void Reset()
        {
            current.AddRange(cleared);
            cleared.Clear();
        }

        public void Sort(Func<ObjectiveComponent,float> evaluation)
        {
            if (AmountLeft <= 0) return;

            current = current.OrderBy(evaluation).ToList();
        }
        #endregion
        #region ICollection Implementation
        public bool IsReadOnly => false;

        public void Add(ObjectiveComponent item)
        {
            current.Add(item);
        }

        public void Clear()
        {
            current.Clear();
            cleared.Clear();
        }

        public bool Contains(ObjectiveComponent item)
        {
            return current.Contains(item) || cleared.Contains(item);
        }

        public void CopyTo(ObjectiveComponent[] array, int arrayIndex)
        {
            current.CopyTo(array, arrayIndex);
        }

        public bool Remove(ObjectiveComponent item)
        {
            if (current.Remove(item)) return true;

            return cleared.Remove(item);
        }

        public IEnumerator<ObjectiveComponent> GetEnumerator()
        {
            return current.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
        #region Operators
        #endregion
        #region Static Methods
        #endregion
    }
}