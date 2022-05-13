using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Globalization;
using ENA;

public class ObjectivesController : MonoBehaviour {

    public Camera screenCam;
    public Vector3 camPosition;


    
    public AudioSource bird;
    public AudioSource dog;
    public AudioSource piano;
    public AudioSource radio;
    public AudioSource cat;
    public AudioSource telefone;

    List<LineRenderer> tracks = new List<LineRenderer>();
    public GameObject door;
    List<Vector3[]> positions = new List<Vector3[]>();
   
    AudioSource audioRescue;

    public ArrayList allObjectives = new ArrayList();
    public ArrayList objectives = new ArrayList();
    public ArrayList allAudios = new ArrayList();
    public ArrayList objectiveAudios = new ArrayList();

    public int stageCount = 0;
    public bool randomObjectives = false;
    public bool enableAllObjectives = false;
    public int numberObjectives = 3;

    public string objetivoatual;

    bool audio1Played;
    bool audio2Played;
    bool audio3Played;
    bool audio4Played;

    float timeAux = 0;
    string sceneName = "";

    public static bool audioFinished;

    bool audiosStarted;

    // Use this for initialization
    void Start()
    {

        //Não entendi nada aqui não :(

        allObjectives.Add(new Objective(" o cachorro", dog,"Dog"));
        allObjectives.Add(new Objective("o pássaro", bird, "Bird"));
        allObjectives.Add(new Objective("o piano", piano, "Piano"));

        if (enableAllObjectives)
        {
            allObjectives.Add(new Objective("o gato", cat, "Cat"));
            allObjectives.Add(new Objective("o telefone", telefone, "Telefone"));
            allObjectives.Add(new Objective("o rádio", radio, "Radio"));

            allObjectives.Add(new Objective("a mesa",null, "Table"));
            allObjectives.Add(new Objective("a televisão",null, "TV"));
            allObjectives.Add(new Objective("o abajur",null, "Lamp"));
            allObjectives.Add(new Objective("o sofá",null, "Sofa"));
            allObjectives.Add(new Objective("a poltrona",null, "Poltrona"));
        }


        stageCount = 0;
        selectObjectives();
        audioRescue = ((Objective)allObjectives[0]).audioRescueSound;

        objetivoatual = ((Objective)allObjectives[stageCount]).nameTTS;

        //if (!enableAllObjectives)
            StartCoroutine(StartSoundObjective(0, 20));


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {
           
            if(stageCount < 3)
            {
                UAP_AccessibilityManager.Say("Seu objetivo é encontrar " + ((Objective)allObjectives[stageCount]).nameTTS, false);
            }
            else
            {
                UAP_AccessibilityManager.Say("Seu objetivo é retornar para a porta", false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if(col.gameObject.tag !="carpet")
        {
           
            if (col.gameObject.name == "Doors" && stageCount == 3)
            {
                
                LineRenderer track = col.gameObject.GetComponent<LineRenderer>();
                List<Vector3> pos = gameObject.GetComponent<MoveTeste>().positions;


                pos.Add(col.transform.position);


                positions.Add(pos.ToArray());
                tracks.Add(track);
                pos.Clear();

                getParcialTime();
                UserModel.time = Time.time - UserModel.time;
                print("Tempo Total: " + UserModel.time);


                UAP_AccessibilityManager.Say("Você encontrou a porta", false);
                UAP_AccessibilityManager.Say("Parabéns, você concluiu o teste", false);

                StartCoroutine(EndGame());

            }

            else if (col.gameObject.name == ((Objective)allObjectives[stageCount]).name)
            {
                audiosStarted = true;

                LineRenderer track = col.gameObject.GetComponent<LineRenderer>();
                List<Vector3> pos = gameObject.GetComponent<MoveTeste>().positions;


                pos.Add(col.transform.position);

                //UserModel.parcialTime[stageCount] = 

                print("Count: " + stageCount);
                positions.Add(pos.ToArray());
                tracks.Add(track);
                pos.Clear();

                getParcialTime();

                UAP_AccessibilityManager.Say("Você encontrou " + ((Objective)allObjectives[stageCount]).nameTTS, false);
                if(!enableAllObjectives)
                    ((Objective)allObjectives[stageCount]).sound.Stop();
               
                stageCount++;

                if(stageCount < 3)
                {
                    UAP_AccessibilityManager.Say("Agora você deve encontrar " + ((Objective)allObjectives[stageCount]).nameTTS, false);
                    if (!enableAllObjectives)
                        StartCoroutine(StartSoundObjective(stageCount,5));
                }
                else
                {
                    UAP_AccessibilityManager.Say("Agora você deve retornar para a porta", false);
                }

                objetivoatual = ((Objective)allObjectives[stageCount]).nameTTS;

                if (stageCount == 4)
                {
                    for (int i = 0; i < tracks.ToArray().Length; i++)
                    {
                        tracks.ToArray()[i].positionCount = positions.ToArray()[i].Length;
                        tracks.ToArray()[i].SetPositions(positions.ToArray()[i]);


                    }


                    UnityEngine.XR.XRSettings.enabled = false;
                    //VRSettings.enabled = false;

                    saveInfos();
                    SceneManager.LoadSceneAsync(BuildIndex.GameplayScene);
                }

                

                //pos.Add(col.transform.position);
                //Fazer Track do Objetivo
            }

        }
       

        
    }

    public void startGame()
    {
        //dog.Play();
        //stageCount++;
        if(stageCount == 0)
            ((Objective)allObjectives[0]).sound.Play();

        audioRescue = ((Objective)allObjectives[stageCount]).audioRescueSound;
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
  
        if (randomObjectives)
            allObjectives = shuffle(allObjectives);

        if (enableAllObjectives)
            allObjectives.RemoveRange(numberObjectives, allObjectives.Count - numberObjectives);

        //allObjectives = shuffle(allObjectives);
        allObjectives.Add(new Objective("Porta", radio, "Doors"));


    }

    public string GetObjectivesNames()
    {
        string names = ((Objective)allObjectives[0]).nameTTS + ", "
            + ((Objective)allObjectives[1]).nameTTS + " e " + ((Objective)allObjectives[2]).nameTTS;

        return names;
    }

    public void currentAudio()
    {
        
    }

    public void resetAudios()
    {
         audio1Played = false;
         audio2Played = false; ;
         audio3Played = false; ;
         audio4Played = false; ;

        audiosStarted = false;
        audioFinished = false;
    }

    public ArrayList shuffle(ArrayList arrayList)
    {
        ArrayList old = arrayList;

        for(int i = 0; i < old.Count; i++)
        {
            int index = Random.Range(0, old.Count);

            Objective temp = (Objective) old[i];
            old[i] = old[index];
            old[index] = temp;
          
        }


        return old;
    }

    public void TakeScreenShoot(string finish)
    {
        //finishOrNot = finish;
        StartCoroutine("shoot");
    }

    public void saveUserStatus()
    {
        string path = Application.persistentDataPath + '/' + UserModel.username + "_" + sceneName + "_timeLog.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(" ");

        System.DateTime currentTime = System.DateTime.Now;

        string time = currentTime.Hour + "_" + currentTime.Minute + "_" + currentTime.Second;
        writer.WriteLine("Usuario: " + UserModel.username + "-" + time);
        writer.WriteLine(" ");

        writer.WriteLine("Tempo Total: " + UserModel.time);
        writer.WriteLine("T1: " + UserModel.parcialTime[0]);
        writer.WriteLine("T2: " + UserModel.parcialTime[1]);
        writer.WriteLine("T3: " + UserModel.parcialTime[2]);
        writer.WriteLine("T4: " + UserModel.parcialTime[3]);
        writer.WriteLine(" ");

        writer.WriteLine("Numero de colisoes na parede: " + UserModel.colisions);
        writer.WriteLine("Ajudas: " + UserModel.helps);

        writer.Close();

    }

    public void saveInfos()
    {
        sceneName = SceneManager.GetActiveScene().name;
        //Directory.CreateDirectory(UserModel.username);
        TakeScreenShoot("Eita");
        saveUserStatus();
    }

    private IEnumerator shoot()
    {
        //GameObject.Find("Text01").GetComponent<Text>().text += "startshoot";
        gameObject.SetActive(false);
        screenCam.gameObject.SetActive(true);
        screenCam.transform.position = camPosition;
        screenCam.transform.rotation.Set(90, 0, 0, 0);

        string day = System.DateTime.Now.Date.ToString().Split(' ')[0];
        day = day.Replace("/", "-");
        string hour = System.DateTime.Now.TimeOfDay.ToString().Split('.')[0];
        string date = day + "_" + hour;
        //totalTime.text = "tempo total:" + Action.gameTime.ToString("F0") + " segundos.";
        //ScreenCapture.CaptureScreenshot(UserModel.username + "/" + sceneName + "_Tracker" + ".png");
        System.DateTime currentTime = System.DateTime.Now;

        string time = currentTime.Hour + "_" + currentTime.Minute + "_" + currentTime.Second;

        ScreenCapture.CaptureScreenshot(UserModel.username + "_" + sceneName + "_" + time + "_Tracker" + ".png");
        yield return new WaitForSeconds(2);

        //screenCam.gameObject.SetActive(false);
        //totalTime.text = " ";
        //Action.gameTime = 0;
        //stage2.Restart();
        //GameObject.Find("Text02").GetComponent<Text>().text += "endtshoot";

    }

    public void getParcialTime()
    {

        if (stageCount == 0)
        {
            
            UserModel.parcialTime[0] = Time.time - UserModel.time;
            print("Parcial: " + UserModel.parcialTime[0]);
        }
        else
        {
            if(stageCount < 4)
            {
                UserModel.parcialTime[stageCount] = Time.time - timeAux;
                print("Parcial: " + UserModel.parcialTime[stageCount]);
            }
           
        }

        timeAux = Time.time;
    }


    private void OnApplicationQuit()
    {

        print("Fechou");
    }

    IEnumerator StartSoundObjective(int index,int time)
    {
        yield return new WaitForSeconds(time);
        //((Objective)allObjectives[index]).sound.Play();
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        for (int i = 0; i < tracks.ToArray().Length; i++)
        {
            tracks.ToArray()[i].positionCount = positions.ToArray()[i].Length;
            tracks.ToArray()[i].SetPositions(positions.ToArray()[i]);
        }


        UnityEngine.XR.XRSettings.enabled = false;
        //VRSettings.enabled = false;

        saveInfos();
        SceneManager.LoadSceneAsync(BuildIndex.GameplayScene);


    }

    public void EndTest()
    {

        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {

        LineRenderer exitTrack = door.GetComponent<LineRenderer>();
        List<Vector3> pos = gameObject.GetComponent<MoveTeste>().positions;
        positions.Add(pos.ToArray());
        tracks.Add(exitTrack);
        pos.Clear();

    
        for (int i = 0; i < tracks.ToArray().Length; i++)
        {
            tracks.ToArray()[i].positionCount = positions.ToArray()[i].Length;
            tracks.ToArray()[i].SetPositions(positions.ToArray()[i]);


        }

        UnityEngine.XR.XRSettings.enabled = false;
        //VRSettings.enabled = false;
        saveInfos();
        yield return new WaitForSeconds(1);

        Application.Quit();


    }
}

public class Objective
{
    public AudioSource hitSound;
    public string nameTTS;
    public AudioSource audioRescueSound;
    public AudioSource sound;
    public string name;


    public Objective(string nameTTS, AudioSource sound,string name)
    {
        //this.hitSound = hitSound;
        this.nameTTS = nameTTS;
        this.sound = sound;
        this.name = name;
       //this.audioRescueSound = audioRescueSound;
    }
}


