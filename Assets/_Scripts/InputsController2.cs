using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputsController2 : MonoBehaviour {

    // Use this for initialization
    public Camera cam;
    public Vector3 camPosition;
    public GameObject player;
   
    public AudioSource BellSound;
    public AudioSource BellAudio;
    /*  public AudioSource AudioNorth;
     public AudioSource AudioNortheast;
     public AudioSource AudioEast;
     public AudioSource AudioSoutheast;
     public AudioSource AudioSouth;
     public AudioSource AudioSouthwest;
     public AudioSource AudioWest;
     public AudioSource AudioNorthwest;
     */
    public AudioSource bellRight;
    public AudioSource bellLeft;

    bool rightZone = true;
    bool leftZone = true;

    AudioListener audioListener;

    private AudioSource objectiveAudio;

    public GameObject doors;

    public GameObject collisionScript;

    public Material walls;
  
    float north = 0;

    private string stringToEdit = "Obrigado por comprar o TTS";

    ObjectivesController script;

    void Start () {
        //Application.LoadLevel(Application.loadedLevel);
        AudioListener.volume = 0.6f;

       // script = player.gameObject.GetComponent<ObjectivesController>();

    }
	
	// Update is called once per frame
	void Update ()
    {

        float cameraAngle = cam.gameObject.transform.eulerAngles.y;
        if (cameraAngle > 87 && cameraAngle <= 93)
        {
            if (rightZone)
            {
                //bellRight.Play();
                rightZone = false;
            }
              
        }
        else
            rightZone = true;


        if (cameraAngle > 267 && cameraAngle <= 273)
        {
            if (leftZone)
            {
                //bellLeft.Play();
                leftZone = false;
            }
        }
        else
            leftZone = true;


        if (Input.GetKeyDown(KeyCode.Escape))
        {

            script.EndTest();
            //Application.Quit();
        }


        if (Input.GetButtonUp("Fire3")){

            if (AudioListener.volume > 0)
                AudioListener.volume = AudioListener.volume - 0.1f;

            /*
                        if(cam.cullingMask ==0)
                            cam.cullingMask = 1;
                        else
                        {
                            cam.cullingMask = 0;
                            //walls.color = Color.black;
                        }
            */
        }
        /*  
      if (Input.GetButtonUp("Fire1"))
      {


          UserModel.helps++;
          float angle = calculateDirection(doors);
          stopAllAudios();

          if (Mathf.Abs(angle) <= 22.5)
          {

              //AudioNorth.Play();
              EasyTTSUtil.SpeechAdd("A porta está a na sua frente");
              print("Norte");
          }
          else if (angle > 22.5 && angle <= 67.5)
          {


              //AudioNortheast.Play();
              EasyTTSUtil.SpeechAdd("A porta está entre o Norte e o Leste");
              print("Nordeste");
          }

          else if (angle > 67.5 && angle <= 112.5)
          {

              //AudioEast.Play();
              EasyTTSUtil.SpeechAdd("A porta está na sua direita");
              print("Leste");
          }

          else if (angle > 112.5 && angle <= 157.5)
          {

              //AudioSouthwest.Play();
              EasyTTSUtil.SpeechAdd("A porta está entre o Leste e o Sul");
              print("Sudeste");
          }

          else if (Mathf.Abs(angle) > 157.5 && Mathf.Abs(angle) <= 180)
          {

              //AudioSouth.Play();
              EasyTTSUtil.SpeechAdd("A porta está na suas costas");
              print("Sul");
          }

          else if (angle > -157.5 && angle <= -112.5)
          {

              //AudioSouthwest.Play();
              EasyTTSUtil.SpeechAdd("A porta está entre o Sul e o Oeste");
              print("Sudoeste");
          }

          else if (angle > -112.5 && angle <= -67.5)
          {


              //AudioWest.Play();
              EasyTTSUtil.SpeechAdd("A porta está na sua esquerda");
              print("Oeste");
          }

          else if (angle > -167.5 && angle <= -22.5)
          {

              //AudioNorthwest.Play();
              EasyTTSUtil.SpeechAdd("A porta está entre o Oeste e o Norte");
              print("Noroeste");
          }


      }
       */
        if (Input.GetButtonUp("Fire2")){

            if (AudioListener.volume < 1)
                AudioListener.volume = AudioListener.volume + 0.1f;


           // stopAllAudios();
           // BellAudio.Play();

         }
        /*
        if (Input.GetAxisRaw("Fire3") != 0)
             {                  

               if (Mathf.Abs(cam.transform.rotation.eulerAngles.y - north) < 5){

                //if(BellAudio.isPlaying)
                //    BellAudio.Stop();
                  stopAllAudios();
                  BellSound.Play();

                  }

             }
*/
        if (Input.touchCount == 2)
        {
            print("Dois dedos");

            if (cam.cullingMask == 0)
                cam.cullingMask = 1;
            else
            {
                cam.cullingMask = 0;
                //walls.color = Color.black;
            }
        }
           
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100),"Direção: " + angle + "Direção2: " + ( angle - cam.transform.eulerAngles.y));
    }

    public float calculateDirection(GameObject gameObj)
    {
        float angle;

        float xPorta = gameObj.transform.position.x;
        float zPorta = gameObj.transform.position.z;

        float dx =(xPorta  - transform.position.x);
        float dz = (zPorta - transform.position.z);

        Vector3 diference = new Vector3(dx,0, dz);

        angle = Vector3.SignedAngle(cam.transform.forward, diference,Vector3.up);
       
        return angle;
    }

    public void stopAllAudios(){

        BellSound.Stop();
        BellAudio.Stop();
       /* AudioNorth.Stop();
        AudioNorthwest.Stop();
        AudioWest.Stop();
        AudioSouthwest.Stop();
        AudioSouth.Stop();
        AudioSoutheast.Stop();
        AudioEast.Stop();
        AudioNortheast.Stop();*/
        
        if (objectiveAudio != null)
            objectiveAudio.Stop();
    }
}
