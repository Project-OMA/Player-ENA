using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleGiroscopio : MonoBehaviour {
	public PlayerControlFix player;

	public float rotFixa,rotAtual;
	private bool gyroEnabled;
	private Gyroscope gyro;

	private Quaternion rot;

	void Start () {
		rotAtual = 0;
		gyroEnabled = EnabledGyro();
	}
	
	void Update () {
//        	transform.localRotation = gyro.attitude * rot;
			player.rotacaoCamera = (int)transform.localEulerAngles.y;
			rotAtual = transform.localEulerAngles.y;
			if(rotAtual >= rotFixa + 90){
				player.contRotacao++;
				rotAtual += 90;
			}else if(rotAtual <= rotFixa - 90){
				player.contRotacao++;
				rotAtual -= 90;
			}
	}
	private bool EnabledGyro(){
		if(SystemInfo.supportsGyroscope){
			gyro = Input.gyro;
			gyro.enabled = true;

			rot = new Quaternion(0,0,1,0);
			return true;
		}

		return false;
	}
	
}
