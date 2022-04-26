using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENA.Input
{
	public class AxisTracker
	{
		#region Variables
		static bool hAtual = false;
		static bool hAnterior = false;
		static bool vAtual = false;
		static bool vAnterior = false;
		#endregion
		#region Methods
		public static bool VerticalDown()
		{
			vAnterior = vAtual;
			vAtual = UnityEngine.Input.GetAxisRaw("Vertical") != 0;
			return vAtual && !vAnterior;
		}

		public static bool HorizontalDown()
		{
			hAnterior = hAtual;
			hAtual = UnityEngine.Input.GetAxisRaw("Horizontal") != 0;
			return hAtual == true && hAnterior == false;
		}
		#endregion
	}
}

public class NewInput: MonoBehaviour {
	public static bool hAtual = false;
	public static bool hAnterior = false;
	public static bool vAtual = false;
	public static bool vAnterior = false;

	public static bool GetAxisVerticalDown(){
		vAnterior = vAtual;
		vAtual = Input.GetAxisRaw("Vertical") != 0;
		return vAtual && !vAnterior;
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
