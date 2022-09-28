using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class MoveTeste : MonoBehaviour {

    // Use this for initialization
    public Camera cam;
    public string imprimir;
    public float stepRange = 1f;
    public float angle;
    float h;
    float v;
    private CharacterController controller;
    public bool canMove = true;
    public Vector3 vectorLepr;
    Vector3 endpostion;
    Vector3 direction;
    public float stepRate = 1F;
    public float stepdelay = 0.5f;

    public List<Vector3> positions;

    public AudioSource stepSound;
    public AudioSource oldStepSound;
    public AudioSource hitSound;

    public AudioClip[] stepClips;
    private int actualStep;
    private int oddStep;

    private Rigidbody rg;

    int movingFoward = -1;

    bool firstStep = false;

    bool soundColider = true;


    void Start()
    {

        rg = GetComponent<Rigidbody>();
        canMove = true;
        stepSound = GetComponent<AudioSource>();     
        positions.Add(transform.position);

        //print("Fase 1: " + UserModel.username);
    }

    // Update is called once per frame
    void Update()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

    }
    private void FixedUpdate()
    {
        direction = new Vector3(0, 0, 0);

        if (v > 0) {
            direction = new Vector3(cam.transform.TransformDirection(Vector3.forward).x, 0, cam.transform.TransformDirection(Vector3.forward).z);
            movingFoward = -1;
        }
            
        // direction = cam.transform.TransformDirection(Vector3.forward) * v + cam.transform.TransformDirection(Vector3.right) * h;
        //
        if (v < 0){
            direction = new Vector3(cam.transform.TransformDirection(Vector3.forward).x * (-1), 0, cam.transform.TransformDirection(Vector3.forward).z * (-1));
            movingFoward = 1;
        }
            

        if (canMove && v != 0)
            MoveStep();


    }

    void SetCanMove()
    {
        canMove = true;
        updatePositions();

    }
    private void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100),"Tempo: " + Time.time);
    }
    public void MoveStep()
    {
        if (!firstStep)
        {
            firstStep = true;
            UserModel.time = Time.time;
            print("Tempo inicial: " + Time.time);
        }

        if(soundColider)
             stepSound.Play();

        if (oddStep == 1)
        {
            oddStep = 2;
        }
        else
        {
            oddStep = 1;
        }

        canMove = false;
        /*
        iTween.MoveAdd(this.gameObject, iTween.Hash(
            "amount", direction * stepRange,
            "time", 0.5f,
            "easetype", iTween.EaseType.linear,
            "oncomplete", "SetCanMove"));
*/

    }

    public void updatePositions()
    {
        positions.Add(transform.position);
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "objects" || col.gameObject.tag == "objetives")
        {

            if (col.gameObject.tag == "objects")
                UserModel.colisions++;


            hitSound.Play();

            if (soundColider)
            {
                
                soundColider = false;
                StartCoroutine(disableSoundColision(2));

            }
/*
            canMove = false;
            iTween.MoveAdd(this.gameObject, iTween.Hash(
                "amount", new Vector3(cam.transform.TransformDirection(Vector3.forward).x * movingFoward, 0, cam.transform.TransformDirection(Vector3.forward).z * movingFoward) * stepRange/4,
                "time", 0.3f,
                "easetype", iTween.EaseType.linear,
                "oncomplete", "SetCanMove"));*/
        }

        else if (col.gameObject.tag == "floor" || col.gameObject.tag == "carpet")
        {
            oldStepSound = stepSound;
            stepSound = col.gameObject.GetComponent<AudioSource>();
        }
          


    }

    IEnumerator disableSoundColision(int time)
    {

        yield return new WaitForSeconds(time);
        soundColider = true;
        // ((Objective)allObjectives[index]).sound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "carpet")
            stepSound = oldStepSound;
    }
}
