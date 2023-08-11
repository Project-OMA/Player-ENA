

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

    public void CreateBoxMeshes()
    {
        //foreach (Wall wall in this.layers.walls)
        for(int i = 0; i < this.layers.walls.Count; i++)
        {
            
            Wall wall = this.layers.walls[i];

            Vector3 start = new Vector3(wall.start[0], 1, -wall.start[1]);
            Vector3 end = new Vector3(wall.end[0] + 1, 1, -wall.end[1] - 1);
            Vector3 center = (end - start) / 2;
            Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 2f, Mathf.Abs(end.z - start.z));

            GameObject wallPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallPiece.transform.position = start;
            wallPiece.transform.position += center;
            wallPiece.transform.localScale = size;
            // change the name
            wallPiece.name = System.String.Format("{0} - start: {1},  end:{2}",i, start.ToString(), end.ToString());
            // Set appropriate material or color for the box

            wallPiece.GetComponent<Renderer>().material.color = Color.red;

            // Optionally, attach the box to a parent object for better organization
            // box.transform.parent = transform;
        }


        //foreach (Floor floor in this.layers.floors)
        for(int i = 0; i < this.layers.floors.Count; i++)
        {
            Floor floor = this.layers.floors[i];
            Vector3 start = new Vector3(floor.start[0], 0, -floor.start[1]);
            Vector3 end = new Vector3(floor.end[0] + 1, 0, -floor.end[1] - 1);
            Vector3 center = (end - start) / 2;
            Vector3 size = new Vector3(Mathf.Abs(end.x - start.x), 1, Mathf.Abs(end.z - start.z));

            //GameObject floorPiece = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // since it is a plane, we divide the size by 10
            //size /= 10; // We are no using the plane anymore, because it has 10 by 190 faces while the cube has 6

            // Create a Mesh with 4 vertices and 2 triangles which share the same vertices
            Mesh mesh = new Mesh();

            // Assign the vertices
            mesh.vertices = new Vector3[] { new Vector3(-0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, 0.5f), new Vector3(0.5f, 0, -0.5f) };
            // Assign the triangles
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            // Assign the UVs
            mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };

            GameObject floorPiece = new GameObject();
            // add the mesh renderer
            MeshRenderer meshRenderer = floorPiece.AddComponent<MeshRenderer>();        
            // Assign the mesh to the MeshFilter
            MeshFilter meshFilter = floorPiece.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // plane collider
            MeshCollider meshCollider = floorPiece.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;

            floorPiece.transform.position = start;
            floorPiece.transform.position += center;
            floorPiece.transform.localScale = size;
            // change the name
             floorPiece.name = System.String.Format("{0} - start: {1},  end:{2}",i, start.ToString(), end.ToString());;
            // Set appropriate material or color for the box
            floorPiece.GetComponent<Renderer>().material.color = Color.blue;
            // Optionally, attach the box to a parent object for better organization
            // box.transform.parent = transform;
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

        //objectiveController.RemoveAll();

        // Parse the JSON string into a Map object
        Map map = Map.CreateFromJSON(rawData);
        //Debug.Log(rawData);
        map.CreateBoxMeshes();
        //BuildMap(map);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
