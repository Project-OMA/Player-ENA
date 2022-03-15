using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControleMenuPrincipal : MonoBehaviour {

    public Toggle giroscopio, oculos, elementos,mapa,vibracao;
    public static bool oculosValue, elementosValue;
    public static bool giroscopioValue, mapaValue,vibrationValue;
    public GameObject buscaXML;
    public InputField nome;
    public static string NomeDoUsuario;
    void Start()
    {
        NomeDoUsuario = "User";
        oculosValue = false;
        elementosValue = false;
        giroscopioValue = false;
        mapaValue = false;    
    }
    public void MudarNome()
    {
        NomeDoUsuario = nome.text;
        print(NomeDoUsuario);
    }
    public void LigarBuscaXML()
    {
        buscaXML.SetActive(true);
    }
    public void DesligarBuscaXML()
    {
        buscaXML.SetActive(false);
    }
    public void AssumirOculus(){
        oculosValue = oculos.isOn;
    }
    public void AssumirGiro(){
        giroscopioValue = giroscopio.isOn;
    }
    public void AssumirElementos(){
        elementosValue = elementos.isOn;
    }
    public void AssumirMapa(){
        mapaValue = mapa.isOn;
    }
    public void AssumirVibracao(){
        vibrationValue = vibracao.isOn;
    }
    public void FalarCarregando(){
        if(Tradutor2.portugues){
		    EasyTTSUtil.SpeechAdd("Carregando");
		}else if(Tradutor2.ingles){
			EasyTTSUtil.SpeechAdd("Loading");
		}else if(Tradutor2.espanhol){
			EasyTTSUtil.SpeechAdd("Cargando");
		}
        print("falando carregando");	
    }
}