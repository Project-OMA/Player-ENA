using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;

namespace ENA.Input
{
	public class ControleGiroscopio: MonoBehaviour
	{
		#region Variables
		public PlayerController player;
		[SerializeField] private float fixedRotation;
		[SerializeField] private float currentRotation;
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
				player.RotationCount++;
				currentRotation += 90;
			} else if(currentRotation <= fixedRotation - 90) {
				player.RotationCount++;
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
