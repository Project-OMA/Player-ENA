using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using ENA.Input;
using ENA.Goals;
using UnityEngine.Serialization;
using UnityEngine.Events;
using ENA.Utilities;

namespace ENA.Maps
{
    public class MapCreator: MonoBehaviour
    {
        #region Constants
        private const string CanvasPath = "/root/canvas";
        private const string NodePath = "/root/layers/layer";
        private const string TilesetPath = "/root/tilesets/tileset";
        public const float TileSizeToUnit = 0.5f;
        public const float WallHeight = 10;
        private const float CeilingHeight = 1.25f;
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
        [FormerlySerializedAs("mineMap")]
        [SerializeField] RectTransform miniMap;
        private string[,,] mapMatrix;
        [Header("Default Tiles")]
        [FormerlySerializedAs("noFloor")]
        [SerializeField] GameObject defaultFloor;
        [SerializeField] GameObject defaultCeiling;
        [SerializeField] GameObject invisibleWall;
        [Header("Map Data")]
        [SerializeField] SpawnObjectsList spawnObjList;
        [SerializeField] Vector2 canvasSize;
        [SerializeField] Vector2 tileSize;
        [SerializeField] Vector2Int matrixSize;
        #endregion
        #region Events
        [Header("Events")]
        public UnityEvent OnLoadedMap;
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
            objectiveController.SortObjectivesBy(playerController.transform.parent.position);
        }

        public void InstantiateTileAt(MapCategory category, string code, int c, int l)
        {
            GameObject prefab;
            Vector3 tileDestination;

            if (spawnObjList.IsNull(code)) {
                switch (category) {
                    case MapCategory.Floor:
                        prefab = defaultFloor;
                        break;
                    case MapCategory.Ceiling:
                        prefab = defaultCeiling;
                        break;
                    default:
                        return;
                }
            } else {
                prefab = spawnObjList.GetPrefab(((int)category), code);
            }

            tileDestination = new Vector3(spawnObjList.distance * l, 0, spawnObjList.distance * c * -1);
            if (category == MapCategory.Ceiling) {
                tileDestination.y += CeilingHeight;
            }

            Debug.Log($"List Index: {category} | Code: {code}");
            if (prefab == null) {
                if (category == MapCategory.CharacterElements) {
                    switch (code) {
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
                    print("O code é: " + code);

                    playerController.transform.parent.position = tileDestination;
                } else {
                    Debug.LogWarning($"{code} [{c},{l}]");
                    //Debug.LogError("Problemas ao pegar o prefab");
                    return;
                }
            } else {
                var newInstance = Instantiate(prefab, tileDestination, prefab.transform.rotation);
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

            miniMap.position = new Vector2(canvasSize.x / 4, 1080 - (canvasSize.y / 4));
            miniMap.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);

            string[] mapLayers = parser.FetchAllItems(NodePath).Select((item) => item.InnerText).ToArray();
            BuildMapMatrix(mapLayers[0], mapLayers[1], mapLayers[2], mapLayers[3], mapLayers[4], mapLayers[5], mapLayers[6], mapLayers[7]);
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
            float XHalfOffset = (matrixSize.x + 1) * HalfTileSizeToUnit;

            invisibleWall.Instance(new Vector3(-TileSizeToUnit, 0, -HalfZOffset), Quaternion.identity, new Vector3(TileSizeToUnit, WallHeight, ZOffset));
            invisibleWall.Instance(new Vector3(XOffset, 0, -HalfZOffset), Quaternion.identity, new Vector3(TileSizeToUnit, WallHeight, ZOffset));
            invisibleWall.Instance(new Vector3(XHalfOffset, 0, TileSizeToUnit), Quaternion.identity, new Vector3(XOffset, WallHeight, TileSizeToUnit));
            invisibleWall.Instance(new Vector3(XHalfOffset, 0, -ZOffset), Quaternion.identity, new Vector3(XOffset, WallHeight, TileSizeToUnit));
        }

        void Start()
        {
            string rawData = "";

            if (!testing) {
                var mapPath = PlayerPrefs.GetString(OptionsPlayer.LoadedMapKey);
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
                    for (int x = 0; x < numberOfLayers; x++) {
                        InstantiateTileAt(LayerOrder[x], mapMatrix[x, c, l], c, l);
                    }
                }
            }
            yield return objectiveController.StartAudios();
            OnLoadedMap?.Invoke();
        }
        #endregion
    }
}
