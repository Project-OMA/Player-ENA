using System;
using ENA.Maps;
using UnityEngine;

namespace ENA.Provenance
{
    [Serializable]
    public partial class Collision: IActivity
    {
        #region Variables
        public string objectID;
        public float timestamp;
        public Vector3 position;
        #endregion
        #region Constructors
        public Collision(string objectID, float timestamp, Vector3 position)
        {
            this.objectID = objectID;
            this.timestamp = timestamp;
            this.position = position;
        }
        #endregion
        #region IActivity Implementation
        public string Name => "collision";
        public Vector3 Position => position;
        public float Timestamp => timestamp;
        #endregion
        #region Static Methods
        public static Collision From(Prop prop, Transform transform) => new(prop.ID, Time.time, transform.position);
        #endregion
    }
}