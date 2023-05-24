using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ENA.Metrics
{
    public partial class SessionModel
    {
        [Serializable]
        public class Objective: EntityFactory
        {
            #region Variables
            public string ObjectiveID;
            public List<Action> Actions;
            public List<Collision> Collisions;
            [SerializeField] float initialTimestamp;
            [SerializeField] string entityID;
            #endregion
            #region Properties
            public float TimeSpent => CalculateTimeSpent();
            public int NumberOfCollisions => Collisions.Count;
            public int NumberOfRotations => ActionCount(Action.Type.Turn);
            public int NumberOfSteps => ActionCount(Action.Type.Walk);
            #endregion
            #region Constructors
            public Objective(string objectiveID, float startingTimestamp)
            {
                entityID = Entity.GenerateEntityID();
                ObjectiveID = objectiveID;
                initialTimestamp = startingTimestamp;
                Actions = new List<Action>();
                Collisions = new List<Collision>();
            }
            #endregion
            #region EntityFactory Implementation
            public Entity GetEntity()
            {
                var entity = new Entity(entityID, ObjectiveID);
                entity.AddProperty(nameof(TimeSpent), TimeSpent);
                entity.AddProperty(nameof(NumberOfCollisions), NumberOfCollisions);
                entity.AddProperty(nameof(NumberOfRotations), NumberOfRotations);
                entity.AddProperty(nameof(NumberOfSteps), NumberOfSteps);
                return entity;
            }
            #endregion
            #region Methods
            private int ActionCount(Action.Type actionType)
            {
                return Actions.Count((action) => action.ActionType == actionType);
            }

            private float CalculateTimeSpent()
            {
                if (Actions.Count == 0) return 0;

                var lastAction = Actions.Last();
                return lastAction.Timestamp - initialTimestamp;
            }
            #endregion
        }
    }
}