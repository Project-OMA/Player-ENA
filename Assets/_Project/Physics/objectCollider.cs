using System.Collections;
using System.Collections.Generic;
using ENA.Goals;
using ENA.Input;
using UnityEngine;

public class objectCollider : MonoBehaviour
{
    public int collisionCount = 0;
    public string[] colliderAlert;

    public string[] names;

    bool soundColider = true;
    bool soundTTS = true;

    int language = 0;

    AudioSource hitsound;

    // Use this for initialization
    void Start()
    {
        hitsound = GetComponent<AudioSource>();
        language = GetLanguage();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void Desligar(){
        gameObject.SetActive(false);
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
        else
        {
            return 1;
        }
    }

    public string GetName()
    {
        return names[GetLanguage()];
    }

    public void Collision()
    {
        print("Colidiu");

        if (!hitsound.isPlaying){
            hitsound.Play();
        }
        if(ObjectiveController.instance.objectives.Count > 0){
            if(gameObject == ObjectiveController.instance.objectives[0]){
                OptionsPlayer.instance.InstanceTracer();
                if(ControleMenuPrincipal.elementosValue){
                    Invoke("Desligar",3);
                }
            }
        }
        
        collisionCount++;

        if (collisionCount >= 3)
        {

            if (soundTTS)
            {
                UAP_AccessibilityManager.Say(colliderAlert[language] + names[language], false);
                soundTTS = false;
                StartCoroutine(disableSoundTTS(3));
            }

            print("Som colidiu objeto");
            collisionCount = 0;
        }
    }
    
    IEnumerator disableSoundTTS(int time)
    {
        yield return new WaitForSeconds(time);
        soundTTS = true;
        // ((Objective)allObjectives[index]).sound.Play();
    }

    IEnumerator disableSoundColision(int time)
    {
        
        yield return new WaitForSeconds(time);
        soundColider = true;
        // ((Objective)allObjectives[index]).sound.Play();
    }
}
