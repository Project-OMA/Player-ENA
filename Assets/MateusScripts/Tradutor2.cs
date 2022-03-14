using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tradutor2 : MonoBehaviour {

    public static bool portugues, ingles, espanhol;
    public string[] lingua = new string[3];
	void Start () {
        DetectarLinguagem();
        if (portugues)
        {
            GetComponent<Text>().text = lingua[0];
        }
        else if (ingles)
        {
            GetComponent<Text>().text = lingua[1];
        }
        else if (espanhol)
        {
            GetComponent<Text>().text = lingua[2];
        }
        else
        {
            GetComponent<Text>().text = lingua[0];
        }


    }

    void DetectarLinguagem()
    {
       if(Application.systemLanguage == SystemLanguage.Portuguese)
       {
            print("portugues");
           portugues = true;
       }
       else if (Application.systemLanguage == SystemLanguage.English)
       {
            print("ingles");
            ingles = true;
       }
       else if (Application.systemLanguage == SystemLanguage.Spanish)
       {
            print("espanhol");
            espanhol = true;
       }
       else
       {
            print("nenhuma, setado ingles");
            ingles = true;
       }
    }
}
