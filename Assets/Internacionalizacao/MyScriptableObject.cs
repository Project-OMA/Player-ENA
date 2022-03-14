using UnityEngine;
using System.Collections;


[System.Serializable]
public struct Tradutor
{
    public string portugues;
    public string espanhol;
    public string ingles;
}

[CreateAssetMenu(fileName = "LanguageDictionary", menuName = "LanguageDictionary", order = 1)]
public class MyScriptableObject : ScriptableObject
{
    public Tradutor[] introducao;
}