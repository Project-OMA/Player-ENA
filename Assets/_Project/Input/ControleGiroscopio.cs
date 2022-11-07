using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENA.Utilities;
using ENA.Metrics;
using System;

namespace ENA.Input
{
	[Obsolete]
	public class ControleGiroscopio: ExtendedMonoBehaviour
	{
		#region Variables
		[Header("References")]
		[SerializeField] PlayerController player;
		[SerializeField] Gyroscope gyro;
		[SerializeField] Vector3 offsetRotation;
		#endregion
		#region MonoBehaviour Lifecycle
		protected override void Awake()
		{
			base.Awake();
			CheckForGyro();
		}

		void Start()
		{
			offsetRotation = player.Transform.eulerAngles + new Vector3(90,0,0);
		}
		
		void Update()
		{
			if (gyro == null) return;

			#if ENABLE_LOG
			Debug.Log($"Altitude: {gyro.attitude}");
			Debug.Log($"Gravity: {gyro.gravity}");
			#endif

			var angle = gyro.AttitudeToUnity().eulerAngles;
			#if ENABLE_LOG
			Debug.Log($"Angle: {angle}");
			#endif

			Transform.eulerAngles = angle + offsetRotation;

			UpdateRotationTracker();
		}
		#endregion
		#region Methods
        private void UpdateRotationTracker()
        {
			var cameraRotation = Transform.eulerAngles;
			var playerRotation = player.Transform.eulerAngles;
			var delta = cameraRotation.y - playerRotation.y;

			if (Mathf.Abs(cameraRotation.x) > 45) return;

			// if (delta >= 90) {
			// 	// player.Rotate(1);
			// } else if (delta <= -90) {
			// 	// player.Rotate(-1);
			// }
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
	}
}
