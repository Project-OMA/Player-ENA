

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
using static System.String;

namespace ENA.Maps
{

    [System.Serializable]
    class Wall
    {
        public string type;
        public int[] start;
        public int[] end;
    }

    [System.Serializable]
    class Floor
    {
        public string type;
        public int[] start;
        public int[] end;
    }

    [System.Serializable]
    class DoorAndWindow
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Furniture
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Utensil
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Electronic
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Goal
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Person
    {
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    class Layers
    {
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
    class Map
    {
        public int[] size;
        public Layers layers;
        //  Create a Map object from the JSON string
        public static Map CreateFromJSON(string jsonString)
        {
            Debug.Log(jsonString);
            return JsonUtility.FromJson<Map>(jsonString);
        }

        public void printMap()
        {
            Debug.Log("printMap");
            foreach (Wall wall in this.layers.walls)
            {
                Debug.Log(wall.type);
                Debug.Log(wall.start[0]);
                Debug.Log(wall.start[1]);
                Debug.Log(wall.end[0]);
                Debug.Log(wall.end[1]);
            }

        }

    }


    class MapBuilder
    {
        private Map map;
        private MapCategory category;
        private GameObject defaultFloor;
        private GameObject defaultCeiling;
        private GameObject invisibleWall;
        private PropTheme theme;

        GameObject floorParent;
        GameObject ceilingParent;
        GameObject wallsParent;
        public MapBuilder(Map map)
        {
            this.map = map;
            this.floorParent = new GameObject("Floors");
            this.ceilingParent = new GameObject("Ceiling");
            this.wallsParent = new GameObject("Walls");
        }
        public MapBuilder addDefaultFloor(GameObject defaultFloor)
        {
            this.defaultFloor = defaultFloor;
            return this;
        }

        public MapBuilder addDefaultCeiling(GameObject defaultCeiling)
        {
            this.defaultCeiling = defaultCeiling;
            return this;
        }

        public MapBuilder addDefaultWall(GameObject invisibleWall)
        {
            this.invisibleWall = invisibleWall;
            return this;
        }

        public MapBuilder addTheme(PropTheme theme)
        {
            this.theme = theme;
            return this;
        }

        private void InstanceFloorTile(string code, int[] startArr, int[] endArr)
        {
            Vector3 start = new Vector3(startArr[0], 0, -startArr[1]);
            Vector3 end = new Vector3(endArr[0] + 1, 0, -endArr[1] - 1);
            Vector3 center = (end - start) / 2;
            Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 1, Mathf.Abs(end.z - start.z));
            GameObject floorPiece = null;
            // Verify if there is a theme for this category and code
            if (theme.GetPrefab(MapCategory.Floor, code, out var spawn))
            {
                floorPiece = spawn.Prefab;
            }
            else
            {
                Debug.LogWarning($"Error: {code} was not found in the theme!");
                floorPiece = defaultFloor;
            }

            if (floorPiece == null)
            {
                Debug.LogWarning($"Error: {code} [{startArr[0]},{startArr[1]}] is invalid!");
                return;
            }
            else
            {
                var newInstance = GameObject.Instantiate(floorPiece, start, Quaternion.identity);
                newInstance.transform.position += center;
                newInstance.transform.localScale = size;

                // change the name
                newInstance.name = System.String.Format("Floor: {0} - start: {1},  end:{2}", code, start.ToString(), end.ToString());

                // make the box a child of the parent
                newInstance.transform.parent = floorParent.transform;

                // get the first child the newInstance

                var child = newInstance.transform.GetChild(0);

                Mesh mesh = new Mesh();

                // Assign the vertices
                mesh.vertices = new Vector3[] { new Vector3(-0.5f, 1, -0.5f), new Vector3(-0.5f, 1, 0.5f), new Vector3(0.5f, 1, 0.5f), new Vector3(0.5f, 1, -0.5f) };
                // Assign the triangles
                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

                // Assign the UVs
                var newUvs = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
                // scale down the UVs by the same factor as the parent object
                for (int i = 0; i < newUvs.Length; i++)
                {
                    newUvs[i] = new Vector2(newUvs[i].x * size.x, newUvs[i].y * size.z);
                }
                // scale up by 2
                float scale = .5f;
                for (int i = 0; i < newUvs.Length; i++)
                {
                    newUvs[i] = new Vector2(newUvs[i].x * scale, newUvs[i].y * scale);
                }
                mesh.uv = newUvs;

                // replace the newInstance mesh with the new mesh
                child.GetComponent<MeshRenderer>().material = floorPiece.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;

                // mesh filter
                child.GetComponent<MeshFilter>().mesh = mesh;
                
                // add mesh collider
                MeshCollider meshCollider = child.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;


            }
        }

        private void InstanceCeilingTile(int[] startArr, int[] endArr)
        {
            float ceilingHeigth = 3;

            Vector3 start = new Vector3(startArr[0], ceilingHeigth, -startArr[1]);
            Vector3 end = new Vector3(endArr[0] + 1, ceilingHeigth, -endArr[1] - 1);
            Vector3 center = (end - start) / 2;
            Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 1, Mathf.Abs(end.z - start.z));
            GameObject ceilingPiece = this.defaultCeiling;

            var newInstance = GameObject.Instantiate(ceilingPiece, start, Quaternion.identity);
            newInstance.transform.position += center;
            newInstance.transform.localScale = size;

            // change the name
            newInstance.name = System.String.Format("Ceiling: - start: {0},  end:{0}", start.ToString(), end.ToString());

            // make the box a child of the parent
            newInstance.transform.parent = ceilingParent.transform;

        }

        private void InstanceWallTile(string code, int[] startArr, int[] endArr)
        {
            Vector3 start = new Vector3(startArr[0], 1, -startArr[1]);
            Vector3 end = new Vector3(endArr[0] + 1, 1, -endArr[1] - 1);
            Vector3 center = (end - start) / 2;
            Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 1, Mathf.Abs(end.z - start.z));
            GameObject wallPiece = null;
            // Verify if there is a theme for this category and code
            if (theme.GetPrefab(MapCategory.Wall, code, out var spawn))
            {
                wallPiece = spawn.Prefab;
            }
            else
            {
                Debug.LogWarning($"Error: {code} was not found in the theme!");
                wallPiece = defaultFloor;
            }

            if (wallPiece == null)
            {
                Debug.LogWarning($"Error: {code} [{startArr[0]},{startArr[1]}] is invalid!");
                return;
            }
            else
            {
                var newInstance = GameObject.Instantiate(wallPiece, start, Quaternion.identity);
                newInstance.transform.position += center;
                newInstance.transform.localScale = size;

                // change the name
                newInstance.name = System.String.Format("Wall: {0} - start: {1},  end:{2}", code, start.ToString(), end.ToString());

                // print all news instances children
                // fix the uv
                // the mesh is the default cube mesh
                // get the first child the newInstance

                var child = newInstance.transform.GetChild(0);

                // make the box a child of the parent
                newInstance.transform.parent = wallsParent.transform;
            }
        }

        private void InstanciateFloors()
        {
            List<Floor> floors = this.map.layers.floors;

            //foreach (Floor floor in floors)
            for (int i = 0; i < floors.Count; i++)
            {
                Floor floor = floors[i];
                InstanceFloorTile(floor.type, floor.start, floor.end);
            }
        }

        private void InstanciateCeilings()
        {
            List<Floor> floors = this.map.layers.floors;

            //foreach (Floor floor in floors)
            for (int i = 0; i < floors.Count; i++)
            {
                Floor floor = floors[i];
                InstanceCeilingTile(floor.start, floor.end);
            }
        }

        private void InstanciateWalls()
        {
            List<Wall> walls = this.map.layers.walls;

            //foreach (Wall wall in walls)
            for (int i = 0; i < walls.Count; i++)
            {
                Wall wall = walls[i];
                InstanceWallTile(wall.type, wall.start, wall.end);
            }
        }

        public void buildMap()
        {
            if (this.map == null)
            {
                Debug.Log("Map is null");
                return;
            }

            if (this.category == null)
            {
                Debug.Log("category is null");
                return;
            }
            if (this.defaultFloor == null)
            {
                Debug.Log("defaultFloor is null");
                return;
            }
            if (this.defaultCeiling == null)
            {
                Debug.Log("defaultCeiling is null");
                return;
            }
            if (this.invisibleWall == null)
            {
                Debug.Log("invisibleWall is null");
                return;
            }
            if (this.theme == null)
            {
                Debug.Log("theme is null");
                return;
            }

            InstanciateFloors();
            // TODO: ENABLE THIS LATER
            //InstanciateCeilings();
            InstanciateWalls();
        }

    }

    public class MapCreatorJson : MonoBehaviour
    {

        #region Constants
        public const float TileSizeToUnit = 1;
        public const float WallHeight = 10;
        private const float CeilingHeight = 3f;
        #endregion

        #region Fields
        [SerializeField] GameObject defaultFloor;
        [SerializeField] GameObject defaultCeiling;
        [SerializeField] GameObject invisibleWall;
        [FormerlySerializedAs("jsonRawFile")]
        [SerializeField] TextAsset testMapFile;
        [Header("Map Data")]
        [SerializeField] PropTheme theme;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            string rawData = "";
            rawData = testMapFile.text;
            Map map = Map.CreateFromJSON(rawData);
            MapBuilder mapBuilder = new MapBuilder(map);
            mapBuilder.addDefaultFloor(defaultFloor).addDefaultCeiling(defaultCeiling).addDefaultWall(invisibleWall).addTheme(theme).buildMap();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}