using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandarRotCamera : MonoBehaviour {
	public PlayerControlFix player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		player.rotacaoCamera = (int)transform.eulerAngles.y;
	}
}
