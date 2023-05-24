using System;
using ENA.Utilities;
using UnityEngine;

namespace ENA.Metrics
{
    public partial class SessionModel
    {
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
    }
}