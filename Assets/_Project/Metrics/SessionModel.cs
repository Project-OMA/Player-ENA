using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Metrics
{
    [Serializable]
    public class SessionModel
    {
        #region Classes
        [Serializable]
        public struct Action
        {
            #region Enums
            public enum Type { Walk = 0, Turn = 1 }
            #endregion
            #region Variables
            public Type ActionType;
            public Direction.Basic Direction;
            public float Timestamp;
            public Vector3 Position;
            #endregion
            #region Constructors
            public Action(Type actionType, Direction.Basic direction, float timestamp, Vector3 position)
            {
                ActionType = actionType;
                Direction = direction;
                Timestamp = timestamp;
                Position = position;
            }
            #endregion
        }
        [Serializable]
        public struct Collision
        {
            #region Variables
            public string ObjectID;
            public float Timestamp;
            public Vector3 Position;
            #endregion
            #region Constructors
            public Collision(string objectID, float timestamp, Vector3 position)
            {
                ObjectID = objectID;
                Timestamp = timestamp;
                Position = position;
            }
            #endregion
        }
        [Serializable]
        public class Objective: EntityFactory
        {
            #region Variables
            public string ObjectiveID;
            public List<Action> Actions;
            public List<Collision> Collisions;
            float initialTimestamp;
            string entityID;
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
        #endregion
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