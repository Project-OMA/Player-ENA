using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using ENA.Input;

public class ObjetiveController: MonoBehaviour
{
    public static ObjetiveController instance;
    //Lista de objetivos do mapa
    public List<GameObject> objetives = new List<GameObject>();
    public InitAudios initAudios;
    public int stageCount;
    string sceneName = "";

    public Camera screenCam;
    public Vector3 camPosition;

    // Use this for initialization
    void Start()
    {
        instance = this;
        stageCount = 0;
        StartObjectiveAudio(3);
        Invoke("AdicionarListaColisions", 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAudios()
    {
        initAudios.checkLanguageAndInitTTS();
    }

    public bool FindObjective()
    {

        StopObjectiveAudio();
        objetives.RemoveAt(0);
        objetives[0].GetComponent<ResonanceAudioSource>().gameObject.GetComponent<AudioSource>().Play();


        if (objetives.Count == 0)
            return false;
        else
        {
            StartCoroutine(StartSoundObjective(1, 5));
            return true;
        }

    }

    void AdicionarListaColisions()
    {
        for (int i = 0; i < objetives.Count + 1; i++)
        {
            UserModel.parcialTime.Add(0);
        }

    }
    public void StopObjectiveAudio()
    {
        Debug.Log("Ativou StopObjectiveAudio");

        if (objetives.Count > 0)
        {
            ResonanceAudioSource audioSource = objetives[0].GetComponentInChildren<ResonanceAudioSource>();
            audioSource.GetComponent<AudioSource>().Stop();
        }
    }

    public void StartObjectiveAudio(int time)
    {
        StartCoroutine(StartSoundObjective(1, time));
    }

    IEnumerator StartSoundObjective(int index, int time)
    {
        yield return new WaitForSeconds(time);
        if (objetives.Count > 0) {
            ResonanceAudioSource audioSource = objetives[0].GetComponentInChildren<ResonanceAudioSource>();
            Debug.Log("Ativou o audio do objetivo");
            audioSource.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayFindObjective()
    {
        initAudios.PlayAudioFoundObjective();
    }

    // public void TakeScreenShoot()
    // {
    //     //finishOrNot = finish;
    //     StartCoroutine("shoot");
    // }

    public void saveUserStatus()
    {
        string path = Application.persistentDataPath + '/' + ControleMenuPrincipal.NomeDoUsuario + "_" + sceneName + "_timeLog.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(" ");

        System.DateTime currentTime = System.DateTime.Now;

        string time = currentTime.Hour + ":" + currentTime.Minute;
        writer.WriteLine("Usuario: " + ControleMenuPrincipal.NomeDoUsuario + "-" + time);
        writer.WriteLine(" ");

        writer.WriteLine("Tempo Total: " + UserModel.time);

        for (int i = 0; i < UserModel.parcialTime.Count; i++)
        {
            writer.WriteLine("T" + i + ": " + UserModel.parcialTime[i]);
        }


        writer.WriteLine(" ");

        writer.WriteLine("Numero de colisoes: " + UserModel.colisions);
        //writer.WriteLine("Ajudas: " + UserModel.helps);

        writer.Close();

    }

    public void saveInfos()
    {
        // sceneName = SceneManager.GetActiveScene().name;
        //Directory.CreateDirectory(UserModel.username);
        //TakeScreenShoot();
        saveUserStatus();
    }
}