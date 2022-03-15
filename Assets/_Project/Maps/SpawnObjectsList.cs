using UnityEngine;
using System.Collections;


//Estrutura do Dicionario de objetos
[System.Serializable]
public struct SpawnObjects {
    public string code;
    public GameObject prefab;
}

public class SpawnObjectsList : ScriptableObject {
    //Distancia entre os objetos na unity
    public float distance;
    //Dicionario de objetos
	//public SpawnObjects[] MapObjects;

    [Header("Floor")]
    public SpawnObjects[] floorObjects;
    [Header("Walls")]
    public SpawnObjects[] wallObjects;
    [Header("Door and Window")]
    public SpawnObjects[] doorWindowObjects;
        [Header("Furniture")]
    public SpawnObjects[] furnitureObjects;
    [Header("Eletronics")]
    public SpawnObjects[] eletronicsObjects;
    [Header("Utensils")]
    public SpawnObjects[] utensilsObjects;
    [Header("Interactive Elements")]
    public SpawnObjects[] interactiveElementsObjects;
    [Header("Character")]
    public SpawnObjects[] characterObjects;


    //Funcao que retorna o prefab
    public GameObject getPrefab (int listIndex,string inputCode){
        SpawnObjects[] actual = getList(listIndex);

        foreach(SpawnObjects objs in actual){
            if(string.Equals(inputCode,objs.code)){
                return objs.prefab;
            }
        } 
        Debug.Log(inputCode);
        return null;
    }

    public bool isNull(string inputCode){
        if(string.Equals(inputCode,"-1")){
            return true;
        }
        return false;

    }

    private SpawnObjects[] getList(int index){
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
}