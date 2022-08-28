using UnityEngine;

namespace ENA.Utilities
{
    public static partial class GameObjectExtensions
    {
        public static GameObject Instance(this GameObject self)
        {
            return GameObject.Instantiate(self);
        }

        public static GameObject Instance(this GameObject self, Vector3 position)
        {
            return GameObject.Instantiate(self, position, self.transform.rotation);
        }

        public static GameObject Instance(this GameObject self, Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(self, position, rotation);
        }

        public static GameObject Instance(this GameObject self, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var gameObj = GameObject.Instantiate(self, position, rotation);
            gameObj.transform.localScale = scale;
            return gameObj;
        }

        public static void SetParent(this GameObject self, Transform transform, bool keepGlobalState = false)
        {
            self.transform.SetParent(transform, keepGlobalState);
        }

        public static void ToggleActive(this GameObject self)
        {
            self.SetActive(!self.activeSelf);
        }
    }
}