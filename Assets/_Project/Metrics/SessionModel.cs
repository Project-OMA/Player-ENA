using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ENA.Metrics
{
    [Serializable]
    public partial class SessionModel
    {
        #region Variables
        [SerializeField] private int UserID;
        [SerializeField] private int MapID;
        [SerializeField] private DateTime SessionDate;
        [SerializeField] private bool ClearedMap;
        [SerializeField] List<Objective> objectives;
        #endregion
        #region Properties
        [SerializeField] private int NumberOfCollisions;
        [SerializeField] private int NumberOfActions;
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
            objectives = new();
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
            NumberOfCollisions++;
        }

        public void Register(Action action)
        {
            if (objectives.Count == 0) return;

            objectives.Last().Actions.Add(action);
            NumberOfActions++;
        }
        #endregion
    }
}