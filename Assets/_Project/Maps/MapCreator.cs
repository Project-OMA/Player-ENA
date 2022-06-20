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

namespace ENA.Maps
{
    public class MapCreator: MonoBehaviour
    {
        //public static bool recomecar;
        public GameObject playerFilho, telaPreta;
        public ObjectiveController objectiveController;
        //Prefabs necessarios, coloqueis eles separados assim pra não precisar mexer no dicionario 
        public GameObject noFloor;
        // precisei fazer dois prefabs pra paredes pq a unity tava scalando o prefab e não as instancias por algum motivo ainda desconhecido.
        public GameObject invisibleWall;
        public GameObject invisibleWall2;
        public RectTransform mineMap;
        //Escolher qual o nó que interessa para ser lido
        private string nodeLocation = "/root/layers/layer";
        //Xml que foi exportado do site
        public TextAsset xmlRawFile;
        //Tamanho do canvas
        private Vector2 canvasSize;
        //Tamanho do tileset
        private Vector2 tileSize;
        //Tamanho da matriz do mapa
        public Vector2 matrixSize;
        //Matriz do chao
        private string[,] floorMatrix;
        //Matriz das paredes 
        private string[,] wallMatrix;
        //Matriz das portas e janelas
        private string[,] doorWindowMatrix;
        //Matriz dos moveis
        private string[,] furnitureMatrix;
        //Matriz dos eletronicos
        private string[,] eletronicsMatrix;
        //Matriz dos utensilios
        private string[,] utensilsMatrix;
        //Matriz dos elementos interativos
        private string[,] interactiveElementsMatrix;

        private string[,] characterElementsMatrix;
        public bool testando;
        //Dicionario
        public SpawnObjectsList spawnObjList;

        public GameObject player;

        Vector3 spawnPositionPlus;

        //Botei o contador como publico pra poder usar ele em outras partes do código
        //e ter um norte de quando a matriz inteira foi feita
        public int counter;


        public List<GameObject> novosObjetivos = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            telaPreta.SetActive(false);

            //Aqui trato o xml para retirar esse bloco que texto que atrapalha na leitura e gero uma copia sem ele.
            TextAsset mapxml = new TextAsset();
            //mapxml = (TextAsset)Resources.Load(), typeof(TextAsset));
            string data = "";
            if (!testando) {
                var mapPath = PlayerPrefs.GetString("MapPath");
                if (File.Exists(mapPath))
                    data = File.ReadAllText(mapPath);
                else
                    return; 
            }
            else
            {
                data = xmlRawFile.text;
            }
            // if (data.Equals(""))
            //      data = xmlRawFile.text;
            string newData = data;
            newData = newData.Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");

            //Inicializar o parse
            objectiveController.objectives = new List<GameObject>();
            parseXmlFile(newData);
        }

        void Update()
        {
            //Reorganiza a lista se a lista de objetivos for maior que 1 e se a matrix inteira já foi construida
            //Como a matriz tem sempre o mesmo tamanho, o valor final do dontador vai ser sempre o mesmo
            if (counter >= 375 && objectiveController.objectives.Count >=1) {
                ReorganizaLista();
            }
        }


        //Funcao feita para fazer a conversao do xml para unity
        void parseXmlFile(string xmlData)
        {
            //Criando e carregando
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new StringReader(xmlData));

            //Aqui pego a lista de nós que foram selecionados no atributo nodeLocation e os nos para saber informacoes do mapa
            XmlNodeList myNodeList = xmlDoc.SelectNodes(nodeLocation);
            XmlNode canvasNode = xmlDoc.SelectSingleNode("/root/canvas");
            XmlNode tileNode = xmlDoc.SelectSingleNode("/root/tilesets/tileset");

            //Recuperando os valores de canvas para descobrir as dimensoes da matriz
            canvasSize = new Vector2(float.Parse(canvasNode.Attributes["width"].Value), float.Parse(canvasNode.Attributes["height"].Value));
            tileSize = new Vector2(float.Parse(tileNode.Attributes["tilewidth"].Value), float.Parse(tileNode.Attributes["tileheight"].Value));
            matrixSize = new Vector2(Mathf.RoundToInt(canvasSize.x / tileSize.x), Mathf.RoundToInt(canvasSize.y / tileSize.y));
            mineMap.position = new Vector2(canvasSize.x / 4, 1080 - (canvasSize.y / 4));
            mineMap.sizeDelta = new Vector2(canvasSize.x / 2, canvasSize.y / 2);
            // txt1.text = myNodeList.Item(0).InnerText;
            //Criar matrizes
            CreateMapMatrix(myNodeList.Item(0).InnerText, myNodeList.Item(1).InnerText, myNodeList.Item(2).InnerText, myNodeList.Item(3).InnerText
            , myNodeList.Item(4).InnerText, myNodeList.Item(5).InnerText, myNodeList.Item(6).InnerText, myNodeList.Item(7).InnerText);

        }


        //Funcao que cria as matrizes do mapa
        void CreateMapMatrix(string floor, string wall, string doorWind, string furniture, string eletronics, string utensils, string interactive, string character)
        {
            //contador
            counter = 0;

            //Inicializando matrizes do tamanho certo
            floorMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            wallMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            doorWindowMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            furnitureMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            eletronicsMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            utensilsMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            interactiveElementsMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];
            characterElementsMatrix = new string[Mathf.RoundToInt(matrixSize.y), Mathf.RoundToInt(matrixSize.x)];

            //Splitando a string recebida no xml para ter os valores separados
            string[] floorValues = floor.Split(',');
            string[] wallValues = wall.Split(',');
            string[] doorWindValues = doorWind.Split(',');
            string[] furnitureValues = furniture.Split(',');
            string[] eletronicsValues = eletronics.Split(',');
            string[] utensilsValues = utensils.Split(',');
            string[] interactiveValues = interactive.Split(',');
            string[] characterValues = character.Split(',');

            //Povoando matrizes 
            for (int c = 0; c < matrixSize.y; c++)
            {
                for (int l = 0; l < matrixSize.x; l++)
                {
                    floorMatrix[c, l] = RemoveBlankChars(floorValues[counter]);
                    wallMatrix[c, l] = RemoveBlankChars(wallValues[counter]);
                    doorWindowMatrix[c, l] = RemoveBlankChars(doorWindValues[counter]);
                    furnitureMatrix[c, l] = RemoveBlankChars(furnitureValues[counter]);
                    eletronicsMatrix[c, l] = RemoveBlankChars(eletronicsValues[counter]);
                    utensilsMatrix[c, l] = RemoveBlankChars(utensilsValues[counter]);
                    interactiveElementsMatrix[c, l] = RemoveBlankChars(interactiveValues[counter]);
                    characterElementsMatrix[c, l] = RemoveBlankChars(characterValues[counter]);

                    counter = counter + 1;
                }
            }
            // InstantiateMap();
            StartCoroutine(InstantiateMap());
            InstantiateWall();

        }

        //função que instancia, posiciona e scala as paredes invisiveis
        public void InstantiateWall()
        {
            //cria instancias diferentes para que um não interfica na scala do outro
            //tem muito o que melhorar nessa função, mas devido ao tempo ela resolve por enquanto :3 
            GameObject wall1 = invisibleWall;
            GameObject wall2 = invisibleWall;
            GameObject wall5 = invisibleWall2;
            GameObject wall4 = invisibleWall2;

            Vector3 wallPosition = new Vector3(-0.035f, 0, -canvasSize.y * 0.008f);
            Instantiate(wall1, wallPosition, Quaternion.identity);
            wall1.transform.localScale = new Vector3(0.2f, 10, canvasSize.y * 0.02f);


            wallPosition = new Vector3(canvasSize.x * 0.016f, 0, -canvasSize.y * 0.008f);
            Instantiate(wall2, wallPosition, Quaternion.identity);
            wall2.transform.localScale = new Vector3(0.2f, 10, canvasSize.y * 0.02f);


            wallPosition = new Vector3(canvasSize.x * 0.008f, 0, -0.035f);
            Instantiate(wall5, wallPosition, Quaternion.identity);
            wall5.transform.localScale = new Vector3(canvasSize.x * 0.02f, 10, 0.2f);

            wallPosition = new Vector3(canvasSize.x * 0.008f, 0, -canvasSize.y * 0.016f);
            Instantiate(wall4, wallPosition, Quaternion.identity);
            wall4.transform.localScale = new Vector3(canvasSize.x * 0.02f, 10, 0.2f);


        }

        //Funcao que instancia o mapa
        public IEnumerator InstantiateMap()
        {
            for (int c = 0; c < matrixSize.y; c++) {
                for (int l = 0; l < matrixSize.x; l++) {
                    InstantiateTileAt(MapCategory.Floor, floorMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.Wall, wallMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.DoorWindow, doorWindowMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.Furniture, furnitureMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.Eletronics, eletronicsMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.Utensils, utensilsMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.Interactive, interactiveElementsMatrix[c, l], c, l);
                    InstantiateTileAt(MapCategory.CharacterElements, characterElementsMatrix[c, l], c, l);
                    //InstantiateTileAt(MapCategory.Ceiling, "-1", c, l);
                }
            }
            yield return objectiveController.StartAudios();
            telaPreta.SetActive(false);
        }

        //Funcao que instancia o objeto referente ao codigo no lugar certo    
        public void InstantiateTileAt(MapCategory category, string code, int c, int l)
        {
            //Checa se é o objeto nulo
            //Condicional para pular a lista de chão, para que caso o usuario tenha esquecido de adicionar chão entre o prefab padrão
            GameObject prefab;
            Vector3 tileDestination;

            if (spawnObjList.isNull(code)) {
                if (category == MapCategory.Floor || category == MapCategory.Ceiling) {
                    prefab = noFloor;
                    tileDestination = new Vector3(spawnObjList.distance * l, 0, spawnObjList.distance * c * -1);
                } else {
                    return;
                }
            } else {
                prefab = spawnObjList.getPrefab(((int)category), code);
                tileDestination = new Vector3(spawnObjList.distance * l, 0, spawnObjList.distance * c * -1);
                spawnPositionPlus = new Vector3((spawnObjList.distance * l), 0, (spawnObjList.distance * c * -1) - .29f);
            }

            if (category == MapCategory.Ceiling) {
                tileDestination.y += 4;
            }

            Debug.Log($"List Index: {category} | Code: {code}");
            if (prefab == null) {
                //Caso o prebab seja o player, seta a posicao do player
                if (category == MapCategory.CharacterElements) {
                    if (string.IsNullOrEmpty(code)) return;
                    print("O code é: " + code);
                    if (code == "0.0") {
                        SetPlayerDirection(0);
                    } else if (code == "1.0") {
                        SetPlayerDirection(90);
                    } else if (code == "2.0") {
                        SetPlayerDirection(180);
                    } else if (code == "3.0") {
                        SetPlayerDirection(270);
                    }

                    player.transform.position = spawnPositionPlus;
                    print("spawnPosition: " + spawnPositionPlus);

                } else {
                    Debug.LogError("Problemas ao pegar o prefab");
                    return;
                }
            } else {
                var newInstance = Instantiate(prefab, tileDestination, prefab.transform.rotation);
                if (category == MapCategory.Interactive) {
                    objectiveController.objectives.Add(newInstance);
                }
            }
        }

        private void SetPlayerDirection(float angleDegrees)
        {
            playerFilho.GetComponent<PlayerController>()?.SetDirection(angleDegrees);
        }


        //Funcao que remove caracteres indesejados do elemento
        public string RemoveBlankChars(string entry)
        {
            string outstring = entry.Replace(" ", string.Empty);
            outstring = outstring.Replace("\n", string.Empty);
            outstring = outstring.Replace("\r", string.Empty);
            return outstring;
        }

        //Função que reorganiza os objetivos
        public void ReorganizaLista()
        {

            Debug.Log("Entrou no ReorganizaLista");
            novosObjetivos = objectiveController.objectives;
            int limite = objectiveController.objectives.Count;
            float distancia;
                
            novosObjetivos = novosObjetivos.OrderBy(
            novosObjetivos => Vector3.Distance(player.transform.position, novosObjetivos.transform.position)
            ).ToList();

            objectiveController.objectives = novosObjetivos;


            for (int i = 0; i < limite; i++)
            {
                distancia = Vector3.Distance(objectiveController.objectives[i].transform.position, player.transform.position);
                Debug.Log("A distancia do " + objectiveController.objectives[i].name + " para o player é " + distancia);
            }
        }
    }
}
