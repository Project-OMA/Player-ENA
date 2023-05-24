using System.Collections;
using System.Collections.Generic;
using ENA.Persistency;
using UnityEngine;

namespace ENA.Maps
{
	public class MiniMapController: MonoBehaviour
	{
		#region Variables
		[SerializeField] SettingsProfile profile;
		#endregion
		#region MonoBehaviour Lifecycle
		private void Start()
		{
			if (!profile.MinimapEnabled) {
				gameObject.SetActive(false);
			}
		}
		#endregion
	}
}
