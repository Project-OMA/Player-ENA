using System;
using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using UnityEngine;

public class InitAudios: MonoBehaviour {
    #region Voice Lines
    public string Intro1Text {get; set;}
    public string Intro2Text {get; set;}
    public string Intro3Text {get; set;}
    public string FoundItemText {get; set;}
    public string NextObjectiveText {get; set;}
    public string ReturnToStartText {get; set;}
    public string Article1 {get; set;}
    public string Article2 {get; set;}
    #endregion
    #region Variables
    public static int numberOfTipsGiven;
    #endregion

    public GameObject player;
    public GameObject button;

    public bool skipIntro = false;
    public ObjectiveController objetiveController;

    #region Methods
    void OnApplicationQuit()
    {
        UAP_AccessibilityManager.StopSpeaking();
    }

    public void PlayAudioFoundObjective()
    {
        string audio = "";

        if(objetiveController.objectives.Count > 1)
            audio = FoundItemText + " " + objetiveController.objectives[0].GetComponent<objectCollider>().GetName() + " " + NextObjectiveText + " " + objetiveController.objectives[1].GetComponent<objectCollider>().GetName();
        else{
            audio = FoundItemText + " " + objetiveController.objectives[0].GetComponent<objectCollider>().GetName() + " " + ReturnToStartText;
            OptionsPlayer.instance.finalizar = true;
        }

        UAP_AccessibilityManager.Say(audio, false);
        print("Audio sendo tocado:" + audio);
    }

    public void PlayIntroMessage()
    {
        string names = "";
        string names2 = "";

        for (int i = 0; i < objetiveController.objectives.Count; i++) {
            var objectiveName = objetiveController.objectives[i].GetComponent<objectCollider>().GetName();
            names += ", " + Article1 + " " + objectiveName;
            names2 += ", " + Article2 + " " + objectiveName;
        }

        string introduction = $"{Intro1Text} {names} {Intro2Text} {names2} {Intro3Text}";
        UAP_AccessibilityManager.Say(introduction, false);
        print(introduction);
    }

    void Start()
    {
        numberOfTipsGiven = 0;
    }

    void Update()
    {
        if (!Input.GetButtonDown("Jump")) return;

        string audio;
        numberOfTipsGiven++;

        if (objetiveController.NumberOfObjectives > 0) {
            var nextObjectiveName = objetiveController.objectives[0].GetComponent<objectCollider>().GetName();
            audio = NextObjectiveText + " " + nextObjectiveName;
        } else {
            audio = ReturnToStartText;
        }

        UAP_AccessibilityManager.Say(audio, false);
        print(audio);
    }
    #endregion
}
