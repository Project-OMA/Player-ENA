using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using JurassicEngine.Persistency;
using System.Threading.Tasks;
using System.Text;
using TMPro;
using JurassicEngine;
using ENA;
using ENA.Input;

public class OptionsPlayer : MonoBehaviour {
	#region Constants
    private const string relativePath = "/sdcard/LogsE3/";
	#endregion
	#region Variables
    public Text path;
	string folderPath = relativePath;
	public static string nomeFase;
	public GameObject menu, telaPreta;
	public new RectTransform camera, print;
	[SerializeField] TextMeshProUGUI textoData;
	public bool finalizar, finalizou;
	public Transform inicialPosition;
	public static OptionsPlayer instance;
	public GameObject cameraVR,cameraNormal,cameraMaster,playerPai;
	public TrailRenderer[] cores;
	public int contador;
	public TrailRenderer qualTracer;
	[SerializeField] PlayerControlFix playerControl;
    [SerializeField] RenderTexture minimap;
	#endregion
	private void Start()
    {
        instance = this;

        ConfigureSaveFolder();
        InstanceTracer();
        ConfigureVRGoggles();
        ConfigureGyroscope();
        ConfigureAccessibility();

		Invoke(nameof(DefinirInicio), 0.5f);
    }

    private void ConfigureAccessibility()
    {
        UAP_AccessibilityManager.RegisterOnPauseToggledCallback(ToggleExitMenu);
        UAP_AccessibilityManager.RegisterOnBackCallback(FecharMenu);
        // if (UAP_AccessibilityManager.IsActive())
        //     UAP_AccessibilityManager.PauseAccessibility(true);
    }

    private void ConfigureVRGoggles()
    {
		var isVREnabled = ControleMenuPrincipal.oculosValue;

        if (isVREnabled) {
            playerControl.Target = cameraVR.transform;
        } else {
            playerControl.Target = cameraNormal.transform;
        }

		cameraNormal.SetActive(!isVREnabled);
		cameraVR.SetActive(isVREnabled);
    }

    private void ConfigureGyroscope()
	{
		var isGyroEnabled = ControleMenuPrincipal.giroscopioValue;

		var cameraVRGyro = cameraVR.GetComponent<ControleGiroscopio>();
		var cameraNormalGyro = cameraNormal.GetComponent<ControleGiroscopio>();

		cameraVRGyro.enabled = isGyroEnabled;
        cameraNormalGyro.enabled = isGyroEnabled;

		if (isGyroEnabled) {
            cameraMaster.transform.localEulerAngles = new Vector3(90, 0, 0);
            cameraMaster.transform.SetParent(playerPai.transform);
        } else {
            cameraMaster.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
	}

    private void ConfigureSaveFolder()
    {
		#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
        folderPath = Application.persistentDataPath + relativePath;
		#endif

        if (!System.IO.Directory.Exists(folderPath)) {
            try {
                System.IO.Directory.CreateDirectory(folderPath);
            } catch (UnauthorizedAccessException uae) {
                Debug.LogWarning(uae);
				Debug.Log("Using Default Persistent Path");
				folderPath = Application.persistentDataPath;
            }
        }

		path.text = folderPath;
    }

    private void DefinirInicio()
	{
		inicialPosition.parent = null;
	}

	private void Update()
	{
		if(ControleMenuPrincipal.giroscopioValue) {
			cameraMaster.transform.localPosition = gameObject.transform.localPosition;
		}
	}

	private void OnDestroy()
	{
        UAP_AccessibilityManager.UnregisterOnPauseToggledCallback(ToggleExitMenu);
		UAP_AccessibilityManager.UnregisterOnBackCallback(FecharMenu);
		// if (UAP_AccessibilityManager.IsEnabled())
		// 	UAP_AccessibilityManager.PauseAccessibility(false);
    }

	public void ToggleExitMenu()
	{
		if (menu.activeInHierarchy) {
			AbrirMenuSair();
		} else {
			FecharMenu();
		}
	}

	public void InstanceTracer()
	{
		if(qualTracer != null) {
			qualTracer.transform.parent = null;
		}

		qualTracer = Instantiate(cores[contador], Vector3.zero, Quaternion.identity);
		qualTracer.transform.parent = gameObject.transform;
		qualTracer.transform.localPosition = Vector3.zero;
		qualTracer.GetComponent<TrailRenderer>().Clear();
		qualTracer.gameObject.SetActive(true);

		contador++;
		if(contador > 11)  {
			contador = 0;
		}
	}

	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "final" && finalizar) {
            EndGame();
        }
    }

    private async void EndGame()
    {
        finalizou = true;
        await SalvarTracer();
    }

    public async Task SaveUserStatus()
    {
        System.DateTime currentTime = System.DateTime.Now;
        string time = currentTime.Hour + "_" + currentTime.Minute;
        string path = folderPath + GetSessionName() + "_Log.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        string contents = BuildLog(time);
        await writer.WriteAsync(contents);
        writer.Close();

        Debug.Log($"Saved Log to Path: {path}");
    }

    private string BuildLog(string time)
    {
        var userName = ControleMenuPrincipal.NomeDoUsuario + "-" + time;
		var stageFileName = PlayerPrefs.GetString("Fase");
        var stageName = stageFileName.Substring(0, Mathf.Max(stageFileName.Length - 4, 0));

        var sb = new StringBuilder();
        sb.AppendLine("\nUsuario: " + userName);
        sb.AppendLine("\nMapa: " + stageName);
        sb.AppendLine("\nTempo Total: " + UserModel.time);
        for (int i = 0; i < UserModel.parcialTime.Count; i++) {
            var partialTimeLog = "T" + i + ": " + UserModel.parcialTime[i];
            sb.AppendLine(partialTimeLog);
        }
        sb.AppendLine("\nNumero de colisoes :" + UserModel.colisions);
        sb.AppendLine("Numero de rotações :" + playerControl.RotationCount);
        sb.AppendLine("Numero de passos :" + playerControl.StepCount);
        sb.AppendLine("Ajudas Tipo1(objetivo): " + InitAudios.ajudaObjetivo.ToString());
        sb.AppendLine("Ajuda Tipo2(ponto inicial): " + DirecaoInicial.ajudaInicial.ToString());

        var contents = sb.ToString();
        return contents;
    }

    public async Task SalvarTracer()
    {
        menu.SetActive(false);
        PlaceCameraOnRenderSpot();
        DeliverEndingMessage();
        Debug.Log($"Started Saving");

        SaveScreenshot();
        Debug.Log($"Saving Screenshot");
        await SaveUserStatus();
        await Task.Delay(100);

        Debug.Log($"Finished Saving");
        while (UAP_AccessibilityManager.IsSpeaking()) {
            await Task.Delay(1000);
        }

        SceneManager.LoadSceneAsync(BuildIndex.MainMenu);
        Debug.Log($"Loading Main Menu...");
    }

    private void SaveScreenshot()
    {
        string screenshotName = GetSessionName() + "_Tracker.png";
        string myScreenshotLocation = folderPath + screenshotName;

        Texture2D texture = minimap.ToTexture2D();
        File.WriteAllBytes(myScreenshotLocation, texture.EncodeToPNG());
        Destroy(texture);

        Debug.Log($"Moved file to Path: {myScreenshotLocation}");
    }

    private string GetSessionName()
    {
		string dateTime = FetchCurrentDateTime();
        return ControleMenuPrincipal.NomeDoUsuario + "_" + dateTime;
    }

    private string FetchCurrentDateTime()
    {
        System.DateTime currentTime = System.DateTime.Now;

        string day = currentTime.Date.ToString().Split(' ')[0];
        day = day.Replace("/", "-");

        string time = currentTime.Hour + "_" + currentTime.Minute;
        DisplayDateTime(currentTime, day);

        return day + "_" + time;
    }

    private void DisplayDateTime(DateTime currentTime, string day)
    {
        string hour = currentTime.TimeOfDay.ToString().Split('.')[0];
        string date = day + "_" + hour;
        textoData.text = date;
        textoData.gameObject.SetActive(true);
    }

    private void DeliverEndingMessage()
	{
		if (finalizou) {
            if (Tradutor2.portugues) {
                UAP_AccessibilityManager.Say("Parabéns, você concluiu sua missão", false);
            } else if (Tradutor2.ingles) {
                UAP_AccessibilityManager.Say("Congratulations, you have completed your mission.", false);
            } else if (Tradutor2.espanhol) {
                UAP_AccessibilityManager.Say("Enhorabuena, usted ha terminado su misión", false);
            }
        } else {
            if (Tradutor2.portugues) {
                UAP_AccessibilityManager.Say("Desistiu... Tente novamente mais tarde!", false);
            } else if (Tradutor2.ingles) {
                UAP_AccessibilityManager.Say("Gave up ... Try again later!", false);
            } else if (Tradutor2.espanhol) {
                UAP_AccessibilityManager.Say("Desistió ... ¡Inténtelo de nuevo más tarde!", false);
            }
        }
	}

    private void PlaceCameraOnRenderSpot()
    {
        camera.gameObject.SetActive(true);
        camera.anchoredPosition = print.anchoredPosition;
        camera.sizeDelta = print.sizeDelta;
    }

    public void AbrirMenuSair()
	{
		menu.SetActive(true);
		playerControl.enabled = false;
		//UAP_AccessibilityManager.PauseAccessibility(false);
	}

	public void FecharMenu()
	{
		menu.SetActive(false);
		playerControl.enabled = true;
		//UAP_AccessibilityManager.PauseAccessibility(true);
	}

	public async void VoltarParaOMenu()
    {
        await SalvarTracer();
	}
}
