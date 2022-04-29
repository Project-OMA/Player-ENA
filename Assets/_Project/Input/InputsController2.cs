using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ENA.Input
{
    [Obsolete]
    public class InputsController2 : MonoBehaviour
    {
        [SerializeField] Camera cam;
        public Vector3 camPosition;
        public GameObject player;
    
        public AudioSource BellSound;
        public AudioSource BellAudio;

        bool rightZone = true;
        bool leftZone = true;
        private AudioSource objectiveAudio;
        ObjectivesController script;

        private void Start()
        {
            AudioListener.volume = 0.6f;
        }
        
        private void Update() {
            float cameraAngle = cam.gameObject.transform.eulerAngles.y;
            if (cameraAngle > 87 && cameraAngle <= 93) {
                if (rightZone) {
                    rightZone = false;
                }
            } else {
                rightZone = true;
            }


            if (cameraAngle > 267 && cameraAngle <= 273) {
                if (leftZone) {
                    leftZone = false;
                }
            } else {
                leftZone = true;
            }


            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape)) {
                script.EndTest();
            }


            if (UnityEngine.Input.GetButtonUp("Fire3")) {
                if (AudioListener.volume > 0)
                    AudioListener.volume = AudioListener.volume - 0.1f;
            }
            
            if (UnityEngine.Input.GetButtonUp("Fire2")) {
                if (AudioListener.volume < 1)
                    AudioListener.volume = AudioListener.volume + 0.1f;
            }

            if (UnityEngine.Input.touchCount == 2) {
                print("Dois dedos");
                cam.cullingMask = cam.cullingMask == 0 ? 1 : 0;
            }
            
        }

        public float CalculateDirection(GameObject gameObj)
        {
            float angle;

            float xPorta = gameObj.transform.position.x;
            float zPorta = gameObj.transform.position.z;

            float dx = xPorta  - transform.position.x;
            float dz = zPorta - transform.position.z;

            Vector3 diference = new Vector3(dx,0, dz);

            angle = Vector3.SignedAngle(cam.transform.forward, diference,Vector3.up);
        
            return angle;
        }

        public void StopAllAudios()
        {
            BellSound.Stop();
            BellAudio.Stop();
            
            if (objectiveAudio != null)
                objectiveAudio.Stop();
        }
    }
}
