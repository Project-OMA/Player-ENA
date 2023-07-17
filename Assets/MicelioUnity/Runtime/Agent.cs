using System;
using System.Collections.Generic;

namespace MicelioUnity
{
    [Serializable]
    public class Agent
    {
        public string agent_id;
        public string name;
        public string role;
        public string type;
        public double position_x;
        public double position_y;
        public double? position_z;
        public Dictionary<string, object> properties;

        public Agent(string id, string name, string type)
        {
            this.agent_id = id;
            this.name = name;
            this.type = type;
            this.properties = new Dictionary<string, object>();
        }

        public static string GenerateAgentID()
        {
            DateTime currentTime = DateTime.Now;
            return "agent-" + currentTime.ToString("ddHHmmss");
        }

        public void SetPosition(double x, double y, double? z = null)
        {
            position_x = x;
            position_y = y;
            position_z = z;
        }

        public void AddProperty(string key, object value)
        {
			properties.Add(key, value);

            // if (value is float x)
            // {
            //     this.properties.Add(key, (double)new decimal(x));
            // }
            // else
            // {
            //     this.properties.Add(key, value);
            // }
        }

        public void SetRole(string role)
        {
            this.role = role;
        }
    }
}