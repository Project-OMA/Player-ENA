

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonUtility = UnityEngine.JsonUtility;
using ENA.Input;
using ENA.Goals;
using UnityEngine.Serialization;
using ENA.Utilities;
using ENA.Services;
using Event = ENA.Utilities.Event;
using ENA.Props;


    [System.Serializable]
    class Wall {
        public int type;
        public int[] start;
        public int[] end;
    }

    [System.Serializable]
    class Floor {
        public int type;
        public int[] start;
        public int[] end;
    }

    [System.Serializable]
    class DoorAndWindow {
        public int[] pos;
        public int type;
    }

    [System.Serializable]
    class Furniture {
        public int[] pos;
        public int type;
    }

    [System.Serializable]
    class Utensil {
        public int[] pos;
        public int type;
    }

    [System.Serializable]
    class Electronic {
        public int[] pos;
        public int type;
    }

    [System.Serializable]
    class Goal {
        public int[] pos;
        public int type;
    }

    [System.Serializable]
    class Person {
        public int[] pos;
        public int type;
    }

    class Layers {
        public List<Wall> walls;
        public List<Floor> floors;
        public List<DoorAndWindow> door_and_windows;
        public List<Furniture> furniture;
        public List<Utensil> utensils;
        public List<Electronic> eletronics;
        public List<Goal> goals;
        public List<Person> persons;
    }

    [System.Serializable]
    class Map {
        public int[] size;
        public Layers layers;
        //  Create a Map object from the JSON string
        public static Map CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Map>(jsonString);
        }
        void printMap(){
            print("Walls: ");
            foreach (Wall wall in walls)
            {
                print(wall.type);
                print(wall.start[0]);
                print(wall.start[1]);
                print(wall.end[0]);
                print(wall.end[1]);
            }

        }
    }

    public class MapCreatorJson : MonoBehaviour
    {
        #region Constants
        public const float TileSizeToUnit = 1;
            public const float WallHeight = 10;
            private const float CeilingHeight = 3f;
            //public readonly MapCategory[] LayerOrder = new MapCategory[9]{
            //    MapCategory.Floor, MapCategory.Wall, MapCategory.DoorWindow, MapCategory.Furniture,
            //    MapCategory.Electronics, MapCategory.Utensils, MapCategory.Interactive, MapCategory.CharacterElements,
            //    MapCategory.Ceiling
            //};
        #endregion

        #region Fields
        [FormerlySerializedAs("jsonRawFile")]
        [SerializeField] TextAsset testMapFile;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            string rawData = "";

            // if (!testing) {
            //     var mapPath = PlayerPrefs.GetString(LocalCache.LoadedMapKey);
            //     if (File.Exists(mapPath))
            //         rawData = File.ReadAllText(mapPath);
            //     else
            //         return; 
            //} else {
                rawData = testMapFile.text;
            //}

            objectiveController.RemoveAll();
            
            // Parse the JSON string into a Map object
            Map map = Map.CreateFromJSON(rawData);

            //BuildMap(map);

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
