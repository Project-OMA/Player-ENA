using System;
using UnityEngine;

namespace ENA.Metrics
{
    public partial class SessionModel
    {
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
    }
}