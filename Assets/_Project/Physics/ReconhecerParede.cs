using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReconhecerParede : MonoBehaviour {

	void Update () {
		transform.Translate( Vector3.forward * Time.deltaTime);
		transform.Translate(-Vector3.forward * Time.deltaTime);
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.layer == 9){
			col.gameObject.GetComponent<BoxCollider>().enabled = false;
		}
	}
}
