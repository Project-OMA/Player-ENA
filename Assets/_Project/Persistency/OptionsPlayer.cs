using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Text;
using TMPro;
using ENA.Utilities;
using ENA;
using ENA.Input;
using ENA.TTS;
using ENA.Persistency;
using ENA.Player;
using ENA.Services;
using ENA.Goals;

public class OptionsPlayer : MonoBehaviour {
	#region Variables (Temporary)
	[SerializeField] Transform inicialPosition;
    [SerializeField] SettingsProfile profile;
    [Header("Cleanup")]
    [SerializeField] CameraManager cameraManager;
    [SerializeField] PathManager pathManager;
	#endregion
	private void Start()
    {
        pathManager.NewPath(gameObject);
        cameraManager.SetVR(profile.VREnabled);
        cameraManager.SetCameraGyro(profile.GyroEnabled);

        inicialPosition.parent = null;
    }

	private void OnTriggerEnter(Collider other)
    {
        var objectiveController = ObjectiveController.instance;
        if (other.tag == "final" && objectiveController.HasFinished) {
            objectiveController.FindObjective();
        }
    }
}
