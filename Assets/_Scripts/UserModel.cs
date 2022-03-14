using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel : MonoBehaviour
{
    public static int countQualTempo;
    static public string username = "Player";
    static public float time = 0;
    static public List<float> parcialTime;
    static public int colisions = 0;
    static public int helps = 0;

    static public float volume = 0.5f;

    static public string mapPath = "";

    // Use this for initialization
    void Start()
    {
        resetModelData();
        parcialTime = new List<float>();
        time = 0;
        countQualTempo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (countQualTempo < parcialTime.Count)
        {
            parcialTime[countQualTempo] += Time.deltaTime;
        }
        time += Time.deltaTime;
    }

    void resetModelData()
    {
        time = 0;
        parcialTime = new List<float>();
        colisions = 0;
        helps = 0;
    }
}