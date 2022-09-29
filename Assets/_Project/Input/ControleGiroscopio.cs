using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;
using ENA.Metrics;

namespace ENA.Input
{
	public class ControleGiroscopio: MonoBehaviour
	{
		#region Variables
		[SerializeField] private float fixedRotation;
		[SerializeField] private float currentRotation;
		[Header("References")]
		[SerializeField] PlayerController player;
		[SerializeField] SessionTracker sessionTracker;
		private Gyroscope gyro;
		#endregion
		#region Methods
		private void Start()
		{
			currentRotation = 0;
			CheckForGyro();
		}
		
		private void Update()
		{
			if (gyro != null) {
				transform.localRotation = gyro.AttitudeToUnity();
			}

			// player.CameraRotation = (int)transform.localEulerAngles.y;
			currentRotation = transform.localEulerAngles.y;

			if(currentRotation >= fixedRotation + 90) {
				sessionTracker.RegisterRotation(true);
				currentRotation += 90;
			} else if(currentRotation <= fixedRotation - 90) {
				sessionTracker.RegisterRotation(false);
				currentRotation -= 90;
			}
		}

		private void CheckForGyro()
		{
			if(SystemInfo.supportsGyroscope) {
				gyro = UnityEngine.Input.gyro;
				gyro.enabled = true;
			}
		}
		#endregion
	}
}
