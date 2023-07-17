using System.Collections.Generic;
using System;

namespace MicelioUnity
{
    [Serializable]
    public class Activity
    {
        public string activity_id;
        public string name;
        public string time;
        public double position_x;
        public double position_y;
        public double? position_z;
        public string influenced_by;
        public Dictionary<string, object> properties;
        public List<Agent> agents;
        public List<Entity> entities;

        public Activity(string name, string time)
        {
            activity_id = GenerateActivityID();
            this.name = name;
            this.time = time;
            agents = new List<Agent>();
            entities = new List<Entity>();
            properties = new Dictionary<string, object>();
        }

        private string GenerateActivityID()
        {
            DateTime currentTime = DateTime.Now;
            return "activity-" + currentTime.ToString("ddHHmmss");
        }

        public void SetPosition(double x, double y, double? z = null)
        {
            position_x = x;
            position_y = y;
            position_z = z;
        }

        public void SetInfluence(string activity_id)
        {
            influenced_by = activity_id;
        }

        public void AddProperty(string key, object value)
        {
            properties.Add(key, value);
        }

        public void AddEntity(IEntityFactory entity, string role = null)
        {
            Entity e = entity.GetEntity();
            if (role != null)
            {
                e.SetRole(role);
            }
            else if (e.role == null)
            {
                throw new ArgumentNullException("[MICELIO] Impossível inserir uma entidade sem uma role definida." +
                " Adicione uma role antes de inserir o objeto ou na criação dele.");
            }
            entities.Add(e);
        }

        public void AddAgent(IAgentFactory agent, string role = null)
        {
            Agent a = agent.GetAgent();
            if (role != null)
            {
                a.SetRole(role);
            }
            else if (a.role == null)
            {
                throw new ArgumentNullException("[MICELIO] Impossível inserir um agente sem uma role definida." +
                " Adicione uma role antes de inserir o objeto ou na criação dele.");
            }
            agents.Add(a);
        }
    }
}