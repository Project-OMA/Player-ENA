using System.Collections;
using System.Collections.Generic;
using ENA.Persistency;
using UnityEngine;

namespace ENA.Input
{
	public class ControleMiniMap : MonoBehaviour
	{
		[SerializeField] SettingsProfile profile;

		private void Start()
		{
			if(!profile.MinimapEnabled) {
				gameObject.SetActive(false);
			}
		}
	}
}
