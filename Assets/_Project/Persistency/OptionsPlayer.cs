using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OptionsPlayer : MonoBehaviour {
    private const string relativePath = "/sdcard/LogsE3/";
    public Text path;
	string folderPath = relativePath;
	public static string nomeFase;
	
	public GameObject menu,telaPreta;
	public new RectTransform camera, print;
	public Text textoData;
	public bool finalizar, finalizou;
	public Transform inicialPosition;
	public static OptionsPlayer instance;
	public GameObject cameraVR,cameraNormal,cameraMaster,playerPai;
	public TrailRenderer[] cores;
	public int contador;
	public TrailRenderer qualTracer;
	PlayerControlFix playerControl;
	void Start () {
		#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX)
		folderPath = Application.persistentDataPath+relativePath;
		#endif
		playerControl = GetComponent<PlayerControlFix>();
		path.text = folderPath;
		instance = this;
		Invoke("DefinirInicio",0.5f);
		InstanceTracer();
		if(ControleMenuPrincipal.oculosValue){
			GetComponent<PlayerControlFix>().target = cameraVR.transform;
			cameraNormal.SetActive(false);
			cameraVR.SetActive(true);
		}
		else{
			GetComponent<PlayerControlFix>().target = cameraNormal.transform;
			cameraVR.SetActive(false);
			cameraNormal.SetActive(true);
		}

		if(ControleMenuPrincipal.giroscopioValue){
			cameraMaster.transform.localEulerAngles =  new Vector3(90,0,0);
			cameraVR.GetComponent<ControleGiroscopio>().enabled = true;
			cameraNormal.GetComponent<ControleGiroscopio>().enabled = true;
			cameraMaster.transform.SetParent(playerPai.transform);
		}else{
			cameraMaster.transform.localEulerAngles =  new Vector3(0,0,0);
			cameraVR.GetComponent<ControleGiroscopio>().enabled = false;
			cameraNormal.GetComponent<ControleGiroscopio>().enabled = false;
		}
	}
	
	void DefinirInicio(){
		inicialPosition.parent = null;
	}
	void Update () {
		if(ControleMenuPrincipal.giroscopioValue){
			cameraMaster.transform.localPosition = gameObject.transform.localPosition;
		}
	}
	public void InstanceTracer(){
		if(qualTracer != null){
			qualTracer.transform.parent = null;
		}
		qualTracer = Instantiate(cores[contador], new Vector3(0, 0, 0), Quaternion.identity);
		qualTracer.transform.parent = gameObject.transform;
		qualTracer.transform.localPosition = Vector3.zero;
		qualTracer.GetComponent<TrailRenderer>().Clear();
		qualTracer.gameObject.SetActive(true);
		contador++;
		if(contador > 11){
			contador = 0;
		}
	}
	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "final" && finalizar)
        {
			finalizou = true;
			StartCoroutine(SalvarTracer());
        }
    }
	public void saveUserStatus()
    {
		System.DateTime currentTime = System.DateTime.Now;
		string day = System.DateTime.Now.Date.ToString().Split(' ')[0];
        day = day.Replace("/", "-");
        string hour = System.DateTime.Now.TimeOfDay.ToString().Split('.')[0];
		string time = currentTime.Hour + "_" + currentTime.Minute;
        string path = folderPath + ControleMenuPrincipal.NomeDoUsuario +"_" + day +"_"+ time +"_Log.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(" ");
        writer.WriteLine("Usuario: " + ControleMenuPrincipal.NomeDoUsuario + "-" + time);
        writer.WriteLine(" ");
		string s = PlayerPrefs.GetString("Fase");
		s = s.Substring(0, s.Length - 4);
		writer.WriteLine("Mapa: " + s);
		writer.WriteLine(" ");
        writer.WriteLine("Tempo Total: " + UserModel.time);

        for (int i =0; i < UserModel.parcialTime.Count; i++)
        {
            writer.WriteLine("T" + i + ": " + UserModel.parcialTime[i]);
        }

      
        writer.WriteLine(" ");

        writer.WriteLine("Numero de colisoes :" + UserModel.colisions);
		writer.WriteLine("Numero de rotações :" + playerControl.contRotacao);
		writer.WriteLine("Numero de passos :" + playerControl.contPassos);
		writer.WriteLine("Ajudas Tipo1(objetivo): " + InitAudios.ajudaObjetivo.ToString());
		writer.WriteLine("Ajuda Tipo2(ponto inicial): " + DirecaoInicial.ajudaInicial.ToString());
        //writer.WriteLine("Ajudas: " + UserModel.helps);

        writer.Close();
    }
	public IEnumerator SalvarTracer(){
		menu.SetActive(false);
		camera.gameObject.SetActive(true);
		camera.position = print.position;
		camera.localScale = print.localScale;
		
		string day = System.DateTime.Now.Date.ToString().Split(' ')[0];
        day = day.Replace("/", "-");
        string hour = System.DateTime.Now.TimeOfDay.ToString().Split('.')[0];
        string date = day + "_" + hour;
		textoData.text = date;
		textoData.gameObject.SetActive(true);
		System.DateTime currentTime = System.DateTime.Now;
        string time = currentTime.Hour + "_" + currentTime.Minute;
		yield return new WaitForSeconds(1);
		string sreenshotname = ControleMenuPrincipal.NomeDoUsuario +"_" + day + "_" + time + "_Tracker" + ".png";
		#if UNITY_EDITOR
		string dataPath = Application.dataPath;
		string myDefaultLocation = dataPath.Substring(0, dataPath.Length - 7) + "/" +sreenshotname;
		#elif UNITY_STANDALONE_OSX
		string myDefaultLocation = Application.dataPath + "/Resources/Data/" +sreenshotname;
		#else
		string myDefaultLocation = Application.dataPath + "/" +sreenshotname;
		#endif
		string myScreenshotLocation = folderPath + sreenshotname;
		// Create the folder beforehand if not exists
		if(!System.IO.Directory.Exists(folderPath))
			System.IO.Directory.CreateDirectory(folderPath);

		// Capture and store the screenshot]
		print ("o datapath é" + Application.persistentDataPath);
		UnityEngine.ScreenCapture.CaptureScreenshot(sreenshotname);
		saveUserStatus();
		yield return new WaitForSeconds(2);
		File.Move(myDefaultLocation, myScreenshotLocation);
		// System.IO.Directory.(myScreenshotLocation);
		if(finalizou){
			if(Tradutor2.portugues){
				EasyTTSUtil.SpeechAdd("Parabéns, você concluiu sua missão");
			}else if(Tradutor2.ingles){
				EasyTTSUtil.SpeechAdd("Congratulations, you have completed your mission.");
			}else if(Tradutor2.espanhol){
				EasyTTSUtil.SpeechAdd("Enhorabuena, usted ha terminado su misión");
			}
		}else{
			if(Tradutor2.portugues){
				EasyTTSUtil.SpeechAdd("Desistiu... Tente novamente mais tarde!");
			}else if(Tradutor2.ingles){
				EasyTTSUtil.SpeechAdd("Gave up ... Try again later!");
			}else if(Tradutor2.espanhol){
				EasyTTSUtil.SpeechAdd("Desistió ... ¡Inténtelo de nuevo más tarde!");
			}
		}
		
		
		SceneManager.LoadScene(1);
	}
	public void AbrirMenuSair(){
		menu.SetActive(true);
	}
	public void FecharMenu(){
		menu.SetActive(false);
	}
	public void VoltarParaOMenu(){
		StartCoroutine(SalvarTracer());
	}
}
