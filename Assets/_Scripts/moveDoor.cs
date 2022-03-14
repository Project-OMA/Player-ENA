﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveDoor : MonoBehaviour
{
	public bool aberta;

    public GameObject door1;
    public GameObject door2;

    public AudioSource open;
    public AudioSource close;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
    	if(other.name == "Player"){
			if(aberta){
				open.Play();
				door1.SetActive(false);
				door2.SetActive(false);
			}
    	}
    }

    void OnTriggerExit(Collider other){
    	if(other.name == "Player"){
			close.Play();
			door1.SetActive(true);
			door2.SetActive(true);
    	}
    }

	void OnTriggerStay(Collider other)
	{
		if(other.name == "Player"){
			if(!aberta){
				if(Input.GetButtonDown("Fire1")){
					open.Play();
					door1.SetActive(false);
					door2.SetActive(false);
				}
			}	
		}
	}
}