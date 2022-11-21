using UnityEngine;
using System.Collections;

namespace ENA.Maps
{
    [System.Serializable]
    public struct SpawnObjects
    {
        #region Variables
        public string code;
        public GameObject prefab;
        #endregion
    }

    public class SpawnObjectsList: ScriptableObject
    {
        #region Variables
        public float distance;
        [Header("Layers")]
        [SerializeField] SpawnObjects[] floorObjects;
        [SerializeField] SpawnObjects[] wallObjects;
        [SerializeField] SpawnObjects[] doorWindowObjects;
        [SerializeField] SpawnObjects[] furnitureObjects;
        [SerializeField] SpawnObjects[] eletronicsObjects;
        [SerializeField] SpawnObjects[] utensilsObjects;
        [SerializeField] SpawnObjects[] interactiveElementsObjects;
        [SerializeField] SpawnObjects[] characterObjects;
        #endregion
        #region Methods
        public GameObject GetPrefab(int listIndex, string inputCode)
        {
            SpawnObjects[] actual = GetList(listIndex);

            foreach(SpawnObjects objs in actual){
                if(string.Equals(inputCode,objs.code)){
                    return objs.prefab;
                }
            }
            #if ENABLE_LOG
            Debug.Log(inputCode);
            #endif
            return null;
        }

        public bool IsNull(string inputCode)
        {
            return string.Equals(inputCode, "-1");
        }

        private SpawnObjects[] GetList(int index)
        {
            switch (index)
            {
                case 1:
                    return floorObjects;
                case 2:
                    return wallObjects;
                case 3:
                    return doorWindowObjects;               
                case 4:
                    return furnitureObjects;
                case 5:
                    return eletronicsObjects;
                case 6:
                    return utensilsObjects;
                case 7:
                    return interactiveElementsObjects;
                case 8:
                    return characterObjects;

                default:
                    return floorObjects;
            }
        }
        #endregion
    }
}