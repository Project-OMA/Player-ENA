using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAudios : MonoBehaviour {

    public AudioSource[] audios;

    public AudioSource[] audiosPt;
    public static int ajudaObjetivo;
    public static bool audioFinished;

    bool audiosStarted;

    public GameObject player;
    public GameObject button;

    public bool skipIntro = false;
    bool skiped = false;
    bool instructionPlay = false;
    public MyScriptableObject dictionary;

    public int language;
    public ObjetiveController objetiveController;

    // Use this for initialization
    void Start()
    {
      ajudaObjetivo = 0;
        //checkLanguageAndInitTTS();

        //playAudios();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")){
            ajudaObjetivo++;
            if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                if(objetiveController.objetives[0] != null){
                    string audio = dictionary.introducao[4].espanhol + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName();
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }else{
                    string audio = "Usted debe volver al punto inicial";
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }
                
            }
            else if (Application.systemLanguage == SystemLanguage.English)
            {
                if(objetiveController.objetives.Count > 0){
                    string audio = dictionary.introducao[4].ingles + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName();
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }else{
                    string audio = "You must return to the starting point";
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }
                
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                if(objetiveController.objetives.Count > 0){
                    string audio = dictionary.introducao[4].portugues + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName();
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }else{
                    string audio = "Você deve retornar ao ponto inicial";
                    UAP_AccessibilityManager.Say(audio, false);
                    print(audio);
                }
                
            }
        }
    }

   
    void OnApplicationQuit()
    {
        //EasyTTSUtil.Stop();
    }

    public void playAudios()
    {
        audiosStarted = true;
    }

    public void checkLanguageAndInitTTS()
    {

        string names = "";
        string names2 = "";
       

        if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            //Outputs into console that the system is Portuguese
            // EasyTTSUtil.Initialize(EasyTTSUtil.Spain);

            for (int i = 0; i < objetiveController.objetives.Count; i++) 
                names = names + ", un " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName();

            for (int i = 0; i < objetiveController.objetives.Count; i++)
                names2 = names2 + ", el " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName();

            print(names);
            string indrotucao = dictionary.introducao[0].espanhol + names + dictionary.introducao[1].espanhol+ names2 + dictionary.introducao[2].espanhol;
            UAP_AccessibilityManager.Say(indrotucao, false);
            print(indrotucao);
        }
        else if(Application.systemLanguage == SystemLanguage.English)
        {
            // EasyTTSUtil.Initialize(EasyTTSUtil.UnitedStates);

            for (int i = 0; i < objetiveController.objetives.Count; i++)
                names = names + " a " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName() + " ";

            for (int i = 0; i < objetiveController.objetives.Count; i++)
                names2 = names2 + " the " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName();

            string indrotucao = dictionary.introducao[0].ingles + names + dictionary.introducao[1].ingles + names2 + dictionary.introducao[2].ingles;
            UAP_AccessibilityManager.Say(indrotucao, false);
            Debug.Log(indrotucao);
        }
        else
        {
            // EasyTTSUtil.Initialize(EasyTTSUtil.Brazil);

            for (int i = 0; i < objetiveController.objetives.Count; i++)
                names = names + " um " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName() + " ";

            for (int i = 0; i < objetiveController.objetives.Count; i++)
                names2 = names2 + " o " + objetiveController.objetives[i].GetComponent<objectCollider>().GetName();

            string indrotucao = dictionary.introducao[0].portugues + names + dictionary.introducao[1].portugues + names2 + dictionary.introducao[2].portugues;
            UAP_AccessibilityManager.Say(indrotucao, false);
            Debug.Log(indrotucao);
        }
    }

    public void PlayAudioFoundObjective()
    {
        string audio = "";
        if (Application.systemLanguage == SystemLanguage.Spanish)
        {
            if(objetiveController.objetives.Count > 1)
                audio = dictionary.introducao[3].espanhol + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[4].espanhol + " " + objetiveController.objetives[1].GetComponent<objectCollider>().GetName();
            else{
                audio = dictionary.introducao[3].espanhol + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[5].espanhol;
                OptionsPlayer.instance.finalizar = true;
            }
            UAP_AccessibilityManager.Say(audio, false);
            print(audio);
        }
        else if (Application.systemLanguage == SystemLanguage.English)
        {
            if (objetiveController.objetives.Count > 1)
                audio = dictionary.introducao[3].ingles + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[4].ingles + " " + objetiveController.objetives[1].GetComponent<objectCollider>().GetName();
            else{
                audio = dictionary.introducao[3].ingles + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[5].ingles;
                OptionsPlayer.instance.finalizar = true;
            }
            UAP_AccessibilityManager.Say(audio, false);
            print(audio);
        }
        else
        {
            if (objetiveController.objetives.Count > 1)
                audio = dictionary.introducao[3].portugues + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[4].portugues + " " + objetiveController.objetives[1].GetComponent<objectCollider>().GetName();
            else{
                audio = dictionary.introducao[3].portugues + " " + objetiveController.objetives[0].GetComponent<objectCollider>().GetName() + " " + dictionary.introducao[5].portugues;
                OptionsPlayer.instance.finalizar = true;
            }
            UAP_AccessibilityManager.Say(audio, false);
            print("Audio sendo tocado:" + audio);
        }

    }

    int GetLanguage()
    {
        if (Application.systemLanguage == SystemLanguage.Portuguese)
        {
            return 0;
        }
        else if (Application.systemLanguage == SystemLanguage.English)
        {
            return 2;
        }
        else if(Application.systemLanguage == SystemLanguage.Spanish)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
