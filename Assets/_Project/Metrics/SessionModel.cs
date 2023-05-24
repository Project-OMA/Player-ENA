using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Metrics
{
    [Serializable]
    public partial class SessionModel
    {
        #region Variables
        [field: SerializeField] public int UserID {get; private set;}
        [field: SerializeField] public int MapID {get; private set;}
        [field: SerializeField] public DateTime SessionDate {get; private set;}
        [field: SerializeField] public bool ClearedMap {get; private set;}
        [SerializeField] List<Objective> objectives;
        #endregion
        #region Properties
        public int NumberOfCollisions => (from objective in objectives select objective.NumberOfCollisions).Sum();
        public int NumberOfRotations => (from objective in objectives select objective.NumberOfRotations).Sum();
        public int NumberOfSteps => (from objective in objectives select objective.NumberOfSteps).Sum();
        public Objective[] Objectives => objectives.ToArray();
        public float TotalTime => (from objective in objectives select objective.TimeSpent).Sum();
        #endregion
        #region Constructors
        public SessionModel(int userID, int mapID)
        {
            UserID = userID;
            MapID = mapID;
            SessionDate = DateTime.Now;
            ClearedMap = false;
            objectives = new List<Objective>();
        }
        #endregion
        #region Methods
        public void MarkAsCleared()
        {
            ClearedMap = true;
        }

        public void Register(Objective objective)
        {
            objectives.Add(objective);
        }

        public void Register(Collision collision)
        {
            if (objectives.Count == 0) return;
            objectives.Last().Collisions.Add(collision);
        }

        public void Register(Action action)
        {
            if (objectives.Count == 0) return;
            objectives.Last().Actions.Add(action);
        }
        #endregion
    }
}