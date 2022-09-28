using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class CollisionController : MonoBehaviour {

   
    public AudioSource dogWin;
    public AudioSource birdWin;
    public AudioSource pianoWin;
    public AudioSource portaWin;

    public AudioSource dogWin2;
    public AudioSource birdWin2;
    public AudioSource pianoWin2;
    public AudioSource portaWin2;

    public AudioSource dogObjetivo;
    public AudioSource birdObjetivo;
    public AudioSource pianoObjetivo;
    public AudioSource portaObjetivo;

    public ResonanceAudioSource bird;
    public ResonanceAudioSource dog;
    public ResonanceAudioSource piano;
    public ResonanceAudioSource radio;
    public ResonanceAudioSource cat;
    public ResonanceAudioSource telefone;

    AudioSource audioRescue;

    public ArrayList allObjectives = new ArrayList();
    public ArrayList objectives = new ArrayList();
    public ArrayList allAudios = new ArrayList();
    public ArrayList objectiveAudios = new ArrayList();

    public int stageCount = -1;



    // Use this for initialization
    void Start () {

        allObjectives.Add("Piano");
        allObjectives.Add("Dog");
        allObjectives.Add("Bird");
        allObjectives.Add("Telefone");
        allObjectives.Add("Radio");
        allObjectives.Add("Cat");

        allAudios.Add(piano);
        allAudios.Add(dog);
        allAudios.Add(bird);
        allAudios.Add(telefone);
        allAudios.Add(radio);
        allAudios.Add(cat);

        audioRescue = dogObjetivo;
        selectObjectives();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnTriggerEnter(Collider col)
    {
/*
        if(col.gameObject.name == "" + allObjectives[stageCount])
        {
            print("Wololo objetivo" + stageCount);
            stageCount++;

        }
*/
        if (col.gameObject.name == "Dog")
        {
            dogWin.Play();
            

            if (stageCount == 0)
            {
                dog.audioSource.volume = 0.2f;
                dogWin2.Play();
                bird.audioSource.Play();
                bird.audioSource.loop = true;
                audioRescue = birdObjetivo;

                stageCount++;
            }
            

        }
      
        else if (col.gameObject.name == "Bird"){

            birdWin.Play();
            

            if (stageCount == 1)
            {
                bird.audioSource.volume = 0.2f;
                birdWin2.Play();
                piano.audioSource.Play();
                piano.audioSource.loop = true;
                audioRescue = pianoObjetivo;
                stageCount++;
            }

           

        }

        else if (col.gameObject.name == "Piano")
        {
            
            pianoWin.Play();
           

            if (stageCount == 2)
            {
                pianoWin2.Play();
                piano.audioSource.volume = 0.2f;
                audioRescue = portaObjetivo;
                stageCount++;
            }
        }

        else if (col.gameObject.name == "Doors" && stageCount == 3){

            portaWin.Play();
            portaWin2.Play();
        }

    }

    public void startGame()
    {
        dog.audioSource.Play();
        stageCount++;

        audioRescue = dogObjetivo;
    }

    public void playAudioRescue()
    {
        audioRescue.Play();
    }

    public AudioSource getAudioRescue()
    {
        return audioRescue;
    }

    public void selectObjectives()
    {
        for(int i = 0; i < 3; i++)
        {
            int index = UnityEngine.Random.Range(0, 6 - i);

            allObjectives.RemoveAt(index);

            allAudios.RemoveAt(index);
        }

        print(" " + allObjectives[0] + allObjectives[1] + allObjectives[2]);
    }
}
