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
	[SerializeField] PlayerController playerControl;
    [SerializeField] RenderTexture minimap;
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
        Debug.Log($"{BuildLog(DateTime.Now, new ENAProfile())}");
    }

	private void OnTriggerEnter(Collider other)
    {
        var objectiveController = ObjectiveController.instance;
        if (other.tag == "final" && objectiveController.HasFinished) {
            EndGame();
        }
    }

    private async void EndGame()
    {
        await SalvarTracer();
    }

    private string BuildLog(DateTime time, ENAProfile profile)
    {
        string timeString = time.Hour + "_" + time.Minute;
        var userName = profile.UserName + "-" + timeString;
		var stageFileName = PlayerPrefs.GetString("Fase");
        var stageName = stageFileName.Substring(0, Mathf.Max(stageFileName.Length - 4, 0));

        var yaml = new YAMLBuilder();
        yaml.Header("Resultado da Sessão");
        yaml.Mapping("Usuario", userName);
        yaml.Mapping("Mapa", stageName);
        yaml.Mapping("TempoTotal", UserModel.time.ToString(), "segundos");
        if (UserModel.parcialTime.Count > 0) {
            yaml.Mapping("Segmentos",null);
            using (var indent = yaml.Indent()) {
                UserModel.parcialTime.ForEach((item) => {
                    yaml.Block(item.ToString(),"segundos");
                });
            }
        }
        yaml.Mapping("NumeroDeColisoes", UserModel.colisions.ToString());
        yaml.Mapping("NumeroDeRotações", playerControl.RotationCount.ToString());
        yaml.Mapping("NumeroDePassos", playerControl.StepCount.ToString());
        yaml.Mapping("Ajudas", null);
        using (var indent = yaml.Indent()) {
            yaml.Mapping("Objetivo", InitAudios.numberOfTipsGiven.ToString());
            yaml.Mapping("PontoInicial", DirecaoInicial.ajudaInicial.ToString());
        }

        var contents = yaml.Output();
        return contents;
    }

    public async Task SalvarTracer()
    {
        var userProfile = profile.LoggedProfile;
        var recordingTime = DateTime.Now;

        Debug.Log($"Started Saving");

        LocalCache.SaveTracker(recordingTime, userProfile, minimap);
        Debug.Log($"Saving Screenshot");
        string logContents = BuildLog(recordingTime, userProfile);
        await LocalCache.SaveLog(recordingTime, userProfile, logContents);
        await Task.Delay(100);

        Debug.Log($"Finished Saving");
        while (UAP_AccessibilityManager.IsSpeaking()) {
            await Task.Delay(1000);
        }

        SceneManager.LoadSceneAsync(BuildIndex.MainMenu);
        Debug.Log($"Loading Main Menu...");
    }
}
