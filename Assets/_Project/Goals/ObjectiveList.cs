using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Event = ENA.Event;

namespace ENA.Goals
{
    [CreateAssetMenu(fileName = "New List", menuName = "ENA/Objective List")]
    public partial class ObjectiveList: ScriptableObject, ICollection<ObjectiveComponent>
    {
        #region Variables
        [SerializeField] List<ObjectiveComponent> current = new();
        [SerializeField] List<ObjectiveComponent> cleared = new();
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
        #region Methods
        public List<string> AllObjectiveNames() => current.ConvertAll(e => e.name);

        public bool Check(ObjectiveComponent objective)
        {
            if (objective != NextObjective) return false;

            Mark(objective);
            return true;
        }

        private void Mark(ObjectiveComponent objective)
        {
            cleared.Add(objective);
            current.Remove(objective);
            OnClearObjective.Invoke(objective);
            objective.StopSound();

            if (AmountLeft <= 0) {
                OnClearAllObjectives.Invoke();
                return;
            }

            NextObjective.PlaySound();
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

        public bool Contains(ObjectiveComponent item) => current.Contains(item) || cleared.Contains(item);
        public void CopyTo(ObjectiveComponent[] array, int arrayIndex) => current.CopyTo(array, arrayIndex);
        public bool Remove(ObjectiveComponent item) => current.Remove(item) && cleared.Remove(item);
        public IEnumerator<ObjectiveComponent> GetEnumerator() => current.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}