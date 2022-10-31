using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;
using ENA.Metrics;

namespace ENA.Input
{
	public class ControleGiroscopio: ExtendedMonoBehaviour
	{
		#region Variables
		[SerializeField] private float fixedRotation;
		[SerializeField] private float currentRotation;
		[Header("References")]
		[SerializeField] PlayerController player;
		[SerializeField] SessionTracker sessionTracker;
		[SerializeField] Gyroscope gyro;
		#endregion
		#region MonoBehaviour Lifecycle
		protected override void Awake()
		{
			base.Awake();
			currentRotation = 0;
			CheckForGyro();
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}

		void OnEnable()
		{
			StartCoroutine(TrackRotation());
		}
		#endregion
		#region Methods
		
		private void Update()
		{
			if (gyro != null) {
				#if ENABLE_LOG
				Debug.Log($"Altitude: {gyro.attitude}");
				Debug.Log($"Gravity: {gyro.gravity}");
				#endif

				Transform.localRotation = gyro.AttitudeToUnity();
			}

			currentRotation = Transform.localEulerAngles.y;
		}

		private void CheckForGyro()
		{
			if(SystemInfo.supportsGyroscope) {
				gyro = UnityEngine.Input.gyro;
				gyro.enabled = true;
			}

			#if ENABLE_LOG
			Debug.Log($"Gyro Supported?: {SystemInfo.supportsGyroscope}");
			#endif
		}
		#endregion
		#region Coroutines
		private readonly WaitForSeconds WaitForNextSample = new WaitForSeconds(1);
		IEnumerator TrackRotation()
		{
			if(currentRotation >= fixedRotation + 90) {
				sessionTracker.RegisterRotation(true);
				currentRotation += 90;
			} else if(currentRotation <= fixedRotation - 90) {
				sessionTracker.RegisterRotation(false);
				currentRotation -= 90;
			}

			yield return WaitForNextSample;
		}
		#endregion
	}
}
