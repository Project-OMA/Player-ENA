using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using ENA.Input;
using ENA.Goals;
using UnityEngine.Serialization;
using ENA.Utilities;
using ENA.Services;
using Event = ENA.Utilities.Event;
using ENA.Props;

namespace ENA.Maps
{
    public class MapCreator: MonoBehaviour
    {
        #region Constants
        private const string CanvasPath = "/root/canvas";
        private const string NodePath = "/root/layers/layer";
        private const string TilesetPath = "/root/tilesets/tileset";
        public const float TileSizeToUnit = 1;
        public const float WallHeight = 10;
        private const float CeilingHeight = 3f;
        public readonly MapCategory[] LayerOrder = new MapCategory[9]{
            MapCategory.Floor, MapCategory.Wall, MapCategory.DoorWindow, MapCategory.Furniture,
            MapCategory.Electronics, MapCategory.Utensils, MapCategory.Interactive, MapCategory.CharacterElements,
            MapCategory.Ceiling
        };
        #endregion
        #region Variables
        [Header("References")]
        [SerializeField] PlayerController playerController;
        [SerializeField] ObjectiveController objectiveController;
        [SerializeField] Transform mapParent;
        [SerializeField] Camera trackerCamera;
        private string[,,] mapMatrix;
        [Header("Default Tiles")]
        [SerializeField] GameObject defaultFloor;
        [SerializeField] GameObject defaultCeiling;
        [SerializeField] GameObject invisibleWall;
        [Header("Map Data")]
        [SerializeField] SpawnObjectsList spawnObjList;
        [SerializeField] PropTheme theme;
        [SerializeField] Vector2 canvasSize;
        [SerializeField] Vector2 tileSize;
        [SerializeField] Vector2Int matrixSize;
        #endregion
        #region Events
        [Header("Events")]
        public Event OnLoadedMap;
        [Header("Debugging")]
        [FormerlySerializedAs("testando")]
        [SerializeField] bool testing;
        [FormerlySerializedAs("xmlRawFile")]
        [SerializeField] TextAsset testMapFile;
        #endregion
        #region Methods
        void BuildMapMatrix(string floor, string wall, string doorWind, string furniture, string electronics, string utensils, string interactive, string character)
        {
            int width = Mathf.RoundToInt(matrixSize.y);
            int height = Mathf.RoundToInt(matrixSize.x);

            mapMatrix = new string[LayerOrder.Length, width, height];
            string[] layers = new string[8]{floor, wall, doorWind, furniture, electronics, utensils, interactive, character};

            int counter = 0;
            for (int x = 0; x < layers.Length; x++) {
                counter = 0;
                string[] layerValues = layers[x].Split(',');
                for (int c = 0; c < matrixSize.y; c++) {
                    for (int l = 0; l < matrixSize.x; l++) {
                        mapMatrix[x, c, l] = layerValues[counter].RemoveBlankChars();
                        counter++;
                    }
                }
            }

            for (int c = 0; c < matrixSize.y; c++) {
                for (int l = 0; l < matrixSize.x; l++) {
                    mapMatrix[8, c, l] = "-1";
                    counter++;
                }
            }

            StartCoroutine(InstanceMap());
            SpawnRoomBounds();
            PlaceTrackerCamera();
            objectiveController.SortObjectivesBy(playerController.transform.parent.position);
        }

        private void DefinePlayerPosition(string inputCode, int column, int line)
        {
            switch (inputCode) {
                case "0.0":
                    SetPlayerDirection(0);
                    break;
                case "1.0":
                    SetPlayerDirection(90);
                    break;
                case "2.0":
                    SetPlayerDirection(180);
                    break;
                case "3.0":
                    SetPlayerDirection(270);
                    break;
                default:
                    return;
            }

            playerController.transform.parent.position = GridPositionFor(column, line);
        }

        private Vector3 GridPositionFor(int column, int line)
        {
            return new Vector3(line, 0, column);
        }

        public void InstanceTile(MapCategory category, string code, int c, int l)
        {
            GameObject prefab = null;
            Vector3 tileDestination, tileRotation;

            if (!theme.GetPrefab(category, code, out var spawn)) {
                switch (category) {
                    case MapCategory.Floor:
                        prefab = defaultFloor;
                        break;
                    case MapCategory.Ceiling:
                        prefab = defaultCeiling;
                        break;
                    case MapCategory.CharacterElements:
                        DefinePlayerPosition(code, c, l);
                        break;
                    default:
                        return;
                }
                tileRotation = Vector3.zero;
            } else {
                prefab = spawn.Prefab;
                tileRotation = spawn.Rotation;
            }


            tileDestination = GridPositionFor(c, l);
            if (category == MapCategory.Ceiling) {
                tileDestination.y += CeilingHeight;
            }

            Debug.Log($"List Index: {category} | Code: {code}");
            if (prefab == null) {
                Debug.LogWarning($"Error: {code} [{c},{l}] is invalid!");
                return;
            } else {
                var newInstance = Instantiate(prefab, tileDestination, Quaternion.Euler(tileRotation));
                newInstance.SetParent(mapParent, true);
                if (category == MapCategory.Interactive) {
                    objectiveController.Add(newInstance);
                }
            }
        }

        private void ParseXmlFile(string xmlData)
        {
            const string MapWidthKey = "width";
            const string MapHeightKey = "height";
            const string TileWidthKey = "tilewidth";
            const string TileHeightKey = "tileheight";

            XMLParser parser = XMLParser.Create(xmlData);
            XmlNode canvasNode = parser.Fetch(CanvasPath);
            XmlNode tileNode = parser.Fetch(TilesetPath);

            canvasSize = new Vector2(canvasNode.GetValue(MapWidthKey).AsFloat(), canvasNode.GetValue(MapHeightKey).AsFloat());
            tileSize = new Vector2(tileNode.GetValue(TileWidthKey).AsFloat(), tileNode.GetValue(TileHeightKey).AsFloat());
            matrixSize = new Vector2Int(Mathf.RoundToInt(canvasSize.x / tileSize.x), Mathf.RoundToInt(canvasSize.y / tileSize.y));

            string[] mapLayers = parser.FetchAllItems(NodePath).Select((item) => item.InnerText).ToArray();
            BuildMapMatrix(mapLayers[0], mapLayers[1], mapLayers[2], mapLayers[3], mapLayers[4], mapLayers[5], mapLayers[6], mapLayers[7]);
        }

        public void PlaceTrackerCamera()
        {
            const float xFactor = 10/6f;

            trackerCamera.orthographicSize = Mathf.Max(matrixSize.x * xFactor, matrixSize.y)*TileSizeToUnit/5;
            trackerCamera.transform.position = new Vector3(matrixSize.x*TileSizeToUnit/2,CeilingHeight*2,matrixSize.y*TileSizeToUnit/2);
        }

        private void SetPlayerDirection(float angleDegrees)
        {
            playerController.SetDirection(angleDegrees);
        }

        public void SpawnRoomBounds()
        {
            const float HalfTileSizeToUnit = TileSizeToUnit/2;
            float ZOffset = matrixSize.y * TileSizeToUnit;
            float HalfZOffset = matrixSize.y * HalfTileSizeToUnit;
            float XOffset = matrixSize.x * TileSizeToUnit;
            float HalfXOffset = matrixSize.x * HalfTileSizeToUnit;

            invisibleWall.Instance(new Vector3(-HalfTileSizeToUnit, 0, HalfZOffset-HalfTileSizeToUnit), Quaternion.identity, new Vector3(HalfTileSizeToUnit, WallHeight, ZOffset));
            invisibleWall.Instance(new Vector3(XOffset-HalfTileSizeToUnit, 0, HalfZOffset-HalfTileSizeToUnit), Quaternion.identity, new Vector3(HalfTileSizeToUnit, WallHeight, ZOffset));
            invisibleWall.Instance(new Vector3(HalfXOffset-HalfTileSizeToUnit, 0, -HalfTileSizeToUnit), Quaternion.identity, new Vector3(XOffset, WallHeight, HalfTileSizeToUnit));
            invisibleWall.Instance(new Vector3(HalfXOffset-HalfTileSizeToUnit, 0, ZOffset-HalfTileSizeToUnit), Quaternion.identity, new Vector3(XOffset, WallHeight, HalfTileSizeToUnit));
        }

        void Start()
        {
            string rawData = "";

            if (!testing) {
                var mapPath = PlayerPrefs.GetString(LocalCache.LoadedMapKey);
                if (File.Exists(mapPath))
                    rawData = File.ReadAllText(mapPath);
                else
                    return; 
            } else {
                rawData = testMapFile.text;
            }

            objectiveController.RemoveAll();
            ParseXmlFile(rawData);
        }
        #endregion
        #region Coroutines
        IEnumerator InstanceMap()
        {
            int numberOfLayers = LayerOrder.Length;
            for (int c = 0; c < matrixSize.y; c++) {
                for (int l = 0; l < matrixSize.x; l++) {
                    var skewedColumn = matrixSize.y - c - 1;
                    for (int x = 0; x < numberOfLayers; x++) {
                        InstanceTile(LayerOrder[x], mapMatrix[x, skewedColumn, l], c, l);
                    }
                }
            }
            yield return objectiveController.StartAudios();
            OnLoadedMap.Invoke();
        }
        #endregion
    }
}
