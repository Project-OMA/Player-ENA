using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInput : MonoBehaviour {
	public static bool hAtual = false;
	public static bool hAnterior = false;
	public static bool vAtual = false;
	public static bool vAnterior = false;

	public static bool GetAxisVerticalDown(){
		vAnterior = vAtual;
		if(Input.GetAxisRaw("Vertical") != 0){
			vAtual = true;
		}else{
			vAtual = false;
		}
		if(vAtual == true && vAnterior == false){
			return true;
		}else{
			return false;
		}
	}
	public static bool GetAxisHorizontalDown(){
		vAnterior = vAtual;
		if(Input.GetAxisRaw("Horizontal") != 0){
			vAtual = true;
		}else{
			vAtual = false;
		}
		if(vAtual == true && vAnterior == false){
			return true;
		}else{
			return false;
		}
	}
}
