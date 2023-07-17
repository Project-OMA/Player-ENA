using System.Collections.Generic;

namespace MicelioUnity
{
    [System.Serializable]
    public class Entity
    {
        public string entity_id;
        public string name;
        public string role;
        public double position_x;
        public double position_y;
        public double? position_z;
        public Dictionary<string, object> properties;

        public Entity(string id, string name)
        {
            entity_id = id;
            this.name = name;
            properties = new Dictionary<string, object>();
        }

        public static string GenerateEntityID()
        {
            System.DateTime currentTime = System.DateTime.Now;
            return "entity-" + currentTime.ToString("ddHHmmss");
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
        }

        public void SetRole(string role)
        {
            this.role = role;
        }
    }
}