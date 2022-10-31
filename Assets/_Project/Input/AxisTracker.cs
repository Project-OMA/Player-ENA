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
			return hAtual && !hAnterior;
		}
		#endregion
	}
}