using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleMiniMap : MonoBehaviour {

	void Start () {
		if(!ControleMenuPrincipal.mapaValue){
			gameObject.SetActive(false);
		}
	}
}
