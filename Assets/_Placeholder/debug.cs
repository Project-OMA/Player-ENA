using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class debug : MonoBehaviour {
	
	void Update () {
		Correcao();
	}
	void Correcao(){
		transform.position = new Vector3(13,8.5f,-7);
		transform.eulerAngles =  new Vector3(90,0,0);
	}
}
