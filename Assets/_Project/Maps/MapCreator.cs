using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapCreator : MonoBehaviour
{
    public static bool recomecar;
    public GameObject playerFilho, telaPreta;
    public ObjetiveController objectiveController;
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
        if (recomecar == false)
        {
            recomecar = true;
            Debug.Log("recomeçar ficou falso, e agora é pro jogo iniciar");
            PlayerPrefs.SetString("MapPath", "");
            SceneManager.LoadScene(1);
            return;
        }
        else
        {
            telaPreta.SetActive(false);
        }
        //Aqui trato o xml para retirar esse bloco que texto que atrapalha na leitura e gero uma copia sem ele.
        TextAsset mapxml = new TextAsset();
        //mapxml = (TextAsset)Resources.Load(), typeof(TextAsset));
        string data = "";
        if (!testando)
        {
            data = System.IO.File.ReadAllText(PlayerPrefs.GetString("MapPath"));
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
        objectiveController.objetives = new List<GameObject>();
        parseXmlFile(newData);
        print("recomeçar: " + recomecar);

    }

    void Update()
    {
        //Reorganiza a lista se a lista de objetivos for maior que 1 e se a matrix inteira já foi construida
        //Como a matriz tem sempre o mesmo tamanho, o valor final do dontador vai ser sempre o mesmo
        if (counter >= 375 && objectiveController.objetives.Count >=1)
        {
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
        for (int c = 0; c < matrixSize.y; c++)
        {
            for (int l = 0; l < matrixSize.x; l++)
            {
                InstantiateTileAt(1, floorMatrix[c, l], c, l);
                // Debug.Log("Linha 1: "+c+"/"+l);
                InstantiateTileAt(2, wallMatrix[c, l], c, l);
                // Debug.Log("Linha 2: "+c+"/"+l);
                InstantiateTileAt(3, doorWindowMatrix[c, l], c, l);
                // Debug.Log("Linha 3: "+c+"/"+l);
                InstantiateTileAt(4, furnitureMatrix[c, l], c, l);
                // Debug.Log("Linha 4: "+c+"/"+l);
                InstantiateTileAt(5, eletronicsMatrix[c, l], c, l);
                // Debug.Log("Linha 5: "+c+"/"+l);
                InstantiateTileAt(6, utensilsMatrix[c, l], c, l);
                // Debug.Log("Linha 6: "+c+"/"+l);
                InstantiateTileAt(7, interactiveElementsMatrix[c, l], c, l);
                // Debug.Log("Linha 7: "+c+"/"+l);
                InstantiateTileAt(8, characterElementsMatrix[c, l], c, l);
                // Debug.Log("Linha 8: "+c+"/"+l);
                // yield return new WaitForSeconds(0.1f);
                // yield return new WaitForEndOfFrame();
                // Debug.Log("pausou");
                // Debug.Break();
            }
            yield return new WaitForSeconds(0.01f);
        }
        objectiveController.StartAudios();
        yield return new WaitForSeconds(2f);
        telaPreta.SetActive(false);
    }
    // public void InstantiateMap()
    // {
    //     for (int c = 0; c < matrixSize.y; c++)
    //     {
    //         for (int l = 0; l < matrixSize.x; l++)
    //         {
    //             InstantiateTileAt(1, floorMatrix[c, l], c, l);
    //             Debug.Log("Linha 1: "+c+"/"+l);
    //             InstantiateTileAt(2, wallMatrix[c, l], c, l);
    //             Debug.Log("Linha 2: "+c+"/"+l);
    //             InstantiateTileAt(3, doorWindowMatrix[c, l], c, l);
    //             Debug.Log("Linha 3: "+c+"/"+l);
    //             InstantiateTileAt(4, furnitureMatrix[c, l], c, l);
    //             Debug.Log("Linha 4: "+c+"/"+l);
    //             InstantiateTileAt(5, eletronicsMatrix[c, l], c, l);
    //             Debug.Log("Linha 5: "+c+"/"+l);
    //             InstantiateTileAt(6, utensilsMatrix[c, l], c, l);
    //             Debug.Log("Linha 6: "+c+"/"+l);
    //             InstantiateTileAt(7, interactiveElementsMatrix[c, l], c, l);
    //             Debug.Log("Linha 7: "+c+"/"+l);
    //             InstantiateTileAt(8, characterElementsMatrix[c, l], c, l);
    //             Debug.Log("Linha 8: "+c+"/"+l);
    //             // yield return new WaitForSeconds(0.1f);
    //             // yield return new WaitForEndOfFrame();
    //             // Debug.Log("pausou");
    //             // Debug.Break();
    //         }
    //     }

    //     objectiveController.StartAudios();
    // }

    //Funcao que instancia o objeto referente ao codigo no lugar certo    
    public void InstantiateTileAt(int listIndex, string code, int c, int l)
    {
        //Checa se é o objeto nulo
        //Condicional para pular a lista de chão, para que caso o usuario tenha esquecido de adicionar chão entre o prefab padrão
        if ((listIndex != 1) && (spawnObjList.isNull(code)))
        {
            return;
        }

        // precisei posicionar fora dos ifs para que todos os processos tenham acesso.
        GameObject prefab;
        Vector3 spawnPosition;

        // resolve o problema de o usuario nao colocar chão
        if ((listIndex == 1) && (spawnObjList.isNull(code)))
        {
            // adiciona o prefab do piso fake, sem modificar o dicionario     
            prefab = noFloor;
            //Calculo de posicao de spawn
            spawnPosition = new Vector3(spawnObjList.distance * l, 0, spawnObjList.distance * c * -1);
        }
        else
        {
            //Olhar no dicionario e ver qual prefab é referente ao code     
            prefab = spawnObjList.getPrefab(listIndex, code);
            //Calculo de posicao de spawn
            spawnPosition = new Vector3(spawnObjList.distance * l, 0, spawnObjList.distance * c * -1);
            //200% Gambiarra, calcula o ponto de Spawn do player forçando a corrigir erros de local
            //Os números foram puro teste
            spawnPositionPlus = new Vector3((spawnObjList.distance * l), 0, (spawnObjList.distance * c * -1) - .29f);



        }

        Debug.Log($"List Index: {listIndex} | Code: {code}");
        if (prefab == null)
        {
            //Caso o prebab seja o player, seta a posicao do player
            if (listIndex == 8)
            {
                print("O code é: " + code);
                if (code == "0.0")
                {
                    playerFilho.transform.localEulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 0, playerFilho.transform.localEulerAngles.z);
                    playerFilho.GetComponent<PlayerControlFix>()._rotate.eulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 0, playerFilho.transform.localEulerAngles.z);
                }
                else if (code == "1.0")
                {
                    playerFilho.transform.localEulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 90, playerFilho.transform.localEulerAngles.z);
                    playerFilho.GetComponent<PlayerControlFix>()._rotate.eulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 90, playerFilho.transform.localEulerAngles.z);
                }
                else if (code == "2.0")
                {
                    playerFilho.transform.localEulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 180, playerFilho.transform.localEulerAngles.z);
                    playerFilho.GetComponent<PlayerControlFix>()._rotate.eulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 180, playerFilho.transform.localEulerAngles.z);
                }
                else if (code == "3.0")
                {
                    playerFilho.transform.localEulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 270, playerFilho.transform.localEulerAngles.z);
                    playerFilho.GetComponent<PlayerControlFix>()._rotate.eulerAngles = new Vector3(playerFilho.transform.localEulerAngles.x, 270, playerFilho.transform.localEulerAngles.z);
                }

                player.transform.position = spawnPositionPlus;
                //print(spawnPosition);
                print("spawnPosition: " + spawnPositionPlus);

            }
            else
            {
                Debug.LogError("Problemas ao pegar o prefab");
                return;
            }
        }
        //Instanciando prefab

        else if (listIndex == 7)
        {
            objectiveController.objetives.Add(Instantiate(prefab, spawnPosition, prefab.transform.rotation));
        }
        else
            Instantiate(prefab, spawnPosition, prefab.transform.rotation);


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
        novosObjetivos = objectiveController.objetives;
        int limite = objectiveController.objetives.Count;
        float distancia;
            
        novosObjetivos = novosObjetivos.OrderBy(
        novosObjetivos => Vector3.Distance(player.transform.position, novosObjetivos.transform.position)
        ).ToList();

        objectiveController.objetives = novosObjetivos;


        for (int i = 0; i < limite; i++)
        {
            distancia = Vector3.Distance(objectiveController.objetives[i].transform.position, player.transform.position);
            Debug.Log("A distancia do " + objectiveController.objetives[i].name + " para o player é " + distancia);
        }
    }

}