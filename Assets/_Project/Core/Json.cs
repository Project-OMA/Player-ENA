using UnityEngine;

namespace ENA
{
    public static class Json
    {
        #region Classes
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
        #endregion
        #region Static Methods
        public static T[] DecodeArray<T>(string json, bool hasKey)
        {
            if (!hasKey) json = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.array;
        }

        public static string Encode<T>(T[] array)
        {
            Wrapper<T> wrapper = new() { array = array };
            return JsonUtility.ToJson(wrapper);
        }

        public static string Encode<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new() { array = array };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
        #endregion
    }
}