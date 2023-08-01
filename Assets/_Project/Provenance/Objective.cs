using System;
using System.Collections.Generic;
using ENA.Maps;
using UnityEngine;

namespace ENA.Provenance
{
    [Serializable]
    public partial class Objective: IEntity
    {
        #region Variables
        [SerializeField] List<Action> actions = new();
        [SerializeField] List<Collision> collisions = new();
        [SerializeField] float endTime;
        [SerializeField] string objectiveID;
        [SerializeField] string objectiveName;
        [SerializeField] float startTime;
        #endregion
        #region Constructors
        public Objective(string objectiveID, string objectiveName)
        {
            this.objectiveID = objectiveID;
            this.objectiveName = objectiveName;
        }
        #endregion
        #region IEntity Implementation
        public string ID => objectiveID;
        public string Name => objectiveName;
        #endregion
        #region Methods
        public void End(float timestamp)
        {
            startTime = timestamp;
        }

        public void Register(Action action)
        {
            actions.Add(action);
        }

        public void Register(Collision collision)
        {
            collisions.Add(collision);
        }

        public void Start(float timestamp)
        {
            startTime = timestamp;
        }
        #endregion
        #region Static Methods
        public static Objective From(Prop prop) => new(prop.ID, prop.Name);
        #endregion
    }
}