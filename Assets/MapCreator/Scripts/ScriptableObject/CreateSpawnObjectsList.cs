//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//
//
////Classe feita para criar um scriptable object pelos menus da unity
//public class CreateSpawnObjectsList {
//    [MenuItem("Assets/Create/Spawn Objects List")]
//    public static SpawnObjectsList  Create()
//    {
//        SpawnObjectsList asset = ScriptableObject.CreateInstance("SpawnObjectsList") as SpawnObjectsList;
//
//        AssetDatabase.CreateAsset(asset, "Assets/SpawnObjectsList.asset");
//        AssetDatabase.SaveAssets();
//        return asset;
//    }
//}