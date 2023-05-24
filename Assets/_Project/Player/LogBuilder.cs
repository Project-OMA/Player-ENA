using System;
using ENA.Input;
using ENA.Services;
using ENA.Accessibility;
using ENA.Utilities;
using UnityEngine;
using ENA.Metrics;

namespace ENA.Player
{
    public static class LogBuilder
    {
        #region Static Methods
        public static void AddAction(YAMLBuilder yaml, SessionModel.Action action)
        {
            var actionID = (int) action.ActionType;
            yaml.Mapping("- ActionID", actionID.ToString(), action.ActionType.ToString());

            using (yaml.Indent()) {
                yaml.Mapping("Direction", action.Direction.ToString());
                yaml.Mapping("Time", action.Timestamp.ToString(), "seconds");
                yaml.Mapping("Position", YAMLBuilder.Parse(action.Position));
            }
        }

        public static void AddCollision(YAMLBuilder yaml, SessionModel.Collision collision)
        {
            yaml.Mapping("- ObjectID", collision.ObjectID);
            using (yaml.Indent()) {
                yaml.Mapping("Time", collision.Timestamp.ToString(), "seconds");
                yaml.Mapping("Position", YAMLBuilder.Parse(collision.Position));
            }
        }

        public static void AddObjective(YAMLBuilder yaml, SessionModel.Objective objective)
        {
            yaml.Mapping("- TotalTime", objective.TimeSpent.ToString(), "seconds");

            using (yaml.Indent()) {
                yaml.Mapping("NumberOfCollisions", objective.NumberOfCollisions.ToString());
                yaml.Mapping("NumberOfRotations", objective.NumberOfRotations.ToString());
                yaml.Mapping("NumberOfSteps", objective.NumberOfSteps.ToString());
                yaml.Mapping("Collisions", null);

                using (yaml.Indent()) {
                    foreach(var collision in objective.Collisions) AddCollision(yaml, collision);
                }

                yaml.Mapping("Actions", null);

                using (yaml.Indent()) {
                    foreach(var action in objective.Actions) AddAction(yaml, action);
                }
            }
        }

        public static string MakeLog(SessionModel model)
        {
            // JSON
            string jsonModel = JsonUtility.ToJson(model);
            return jsonModel;
            
            // YAML
            // var yaml = new YAMLBuilder();
            // yaml.Header("Resultado da Sessão");
            // yaml.Mapping("UserID", model.UserID.ToString());
            // yaml.Mapping("MapID", model.MapID.ToString());
            // yaml.Mapping("Date", $"{model.SessionDate.ToShortDateString()} {model.SessionDate.ToShortTimeString()}");
            // yaml.Mapping("Completed", model.ClearedMap.ToString());
            // yaml.Mapping("TotalTime", model.TotalTime.ToString(), "seconds");
            // yaml.Mapping("NumberOfCollisions", model.NumberOfCollisions.ToString());
            // yaml.Mapping("NumberOfRotations", model.NumberOfRotations.ToString());
            // yaml.Mapping("NumberOfSteps", model.NumberOfSteps.ToString());
            // yaml.Mapping("Objectives", null);

            // using (yaml.Indent()) {
            //     foreach(var objective in model.Objectives) AddObjective(yaml, objective);
            // }

            // return yaml.Output();
        }
        #endregion
    }
}