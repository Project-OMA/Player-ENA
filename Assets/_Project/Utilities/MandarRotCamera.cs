using System.Collections;
using System.Collections.Generic;
using ENA.Input;
using UnityEngine;

public class MandarRotCamera : MonoBehaviour {
	public PlayerControlFix player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		player.CameraRotation = (int)transform.eulerAngles.y;
	}
}
