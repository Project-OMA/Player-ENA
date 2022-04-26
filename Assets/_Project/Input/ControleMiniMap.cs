using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENA.Input
{
	public class ControleMiniMap : MonoBehaviour
	{
		private void Start()
		{
			if(!ControleMenuPrincipal.mapaValue) {
				gameObject.SetActive(false);
			}
		}
	}
}
