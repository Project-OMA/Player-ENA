using System;
using UnityEngine;

namespace ENA.Provenance
{
    [Serializable]
    public partial class Action: IActivity
    {
        #region Enums
        public enum Type { Walk = 0, Turn = 1 }
        #endregion
        #region Variables
        [SerializeField] Type actionType;
        [SerializeField] Direction.Basic direction;
        [SerializeField] float timestamp;
        [SerializeField] Vector3 position;
        #endregion
        #region Properties
        public Type ActionType => actionType;
        public Direction.Basic Direction => direction;
        #endregion
        #region Constructors
        public Action(Type actionType, Direction.Basic direction, float timestamp, Vector3 position)
        {
            this.actionType = actionType;
            this.direction = direction;
            this.timestamp = timestamp;
            this.position = position;
        }
        #endregion
        #region IActivity Implementation
        public string Name => actionType.ToString();
        public Vector3 Position => position;
        public float Timestamp => timestamp;
        #endregion
        #region Static Properties
        public static Action From(Type actionType, Direction.Basic direction, Vector3 position) => new(actionType, direction, Time.timeSinceLevelLoad, position);

        public static Action Turn(Direction.Basic direction, Vector3 position)
        {
            return From(Type.Turn, direction, position);
        }

        public static Action Walk(Vector3 startPosition, Vector3 endPosition, Vector3 referenceDirection)
        {
            Vector3 currentDirection = endPosition - startPosition;
            var direction = ENA.Direction.DetermineDirection(referenceDirection, currentDirection);

            return From(Type.Walk, direction, endPosition);
        }
        #endregion
    }
}