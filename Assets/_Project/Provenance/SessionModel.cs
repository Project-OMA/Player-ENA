using System;
using System.Collections.Generic;
using UnityEngine;

namespace ENA.Provenance
{
    [Serializable]
    public partial class SessionModel
    {
        #region Constants
        public const int VERSION = 1;
        #endregion
        #region Variables
        [SerializeField] int mapID;
        [SerializeField] List<Objective> objectives = new();
        [SerializeField] Results results = null;
        [SerializeField] DateTime sessionDate;
        [SerializeField] int userID;
        #endregion
        #region Properties
        public bool HasObjectives => objectives.Count > 0;
        public Objective CurrentObjective => objectives[^1];
        #endregion
        #region Constructors
        public SessionModel(int userID = -1, int mapID = -1)
        {
            InitializeParameters(userID, mapID);
        }
        #endregion
        #region Methods
        private float GetTimestamp() => Time.time;

        private void InitializeParameters(int userID, int mapID)
        {
            this.userID = userID;
            this.mapID = mapID;
            sessionDate = DateTime.Now;
        }

        public void GenerateResults(bool clearedMap)
        {
            var timestamp = GetTimestamp();
            CurrentObjective.End(timestamp);
            results = new Results(clearedMap, timestamp);
        }

        public void Register(Objective objective)
        {
            var timestamp = GetTimestamp();
            if (HasObjectives) CurrentObjective.End(timestamp);
            objective.Start(timestamp);
            objectives.Add(objective);
        }

        public void Register(Collision collision)
        {
            if (!HasObjectives) return;

            CurrentObjective.Register(collision);
        }

        public void Register(Action action)
        {
            if (!HasObjectives) return;

            CurrentObjective.Register(action);
        }
        #endregion
    }
}