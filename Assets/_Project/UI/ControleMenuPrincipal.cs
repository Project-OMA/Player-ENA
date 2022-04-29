using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.Input
{
    public class ControleMenuPrincipal: MonoBehaviour
    {
        #region Variables
        public GameObject buscaXML;
        public TMP_InputField nome;
        #endregion
        #region Static Variables
        public static string NomeDoUsuario = "User";
        public static bool oculosValue = false;
        public static bool elementosValue = false;
        public static bool giroscopioValue = false;
        public static bool mapaValue = false;
        public static bool vibrationValue = false;
        #endregion
        #region Methods
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

        public void AssumirOculus(bool value)
        {
            oculosValue = value;
        }

        public void AssumirGiro(bool value)
        {
            giroscopioValue = value;
        }

        public void AssumirElementos(bool value)
        {
            elementosValue = value;
        }

        public void AssumirMapa(bool value)
        {
            mapaValue = value;
        }

        public void AssumirVibracao(bool value)
        {
            vibrationValue = value;
        }

        public void FalarCarregando()
        {
            if(Tradutor2.portugues){
                EasyTTSUtil.SpeechAdd("Carregando");
            }else if(Tradutor2.ingles){
                EasyTTSUtil.SpeechAdd("Loading");
            }else if(Tradutor2.espanhol){
                EasyTTSUtil.SpeechAdd("Cargando");
            }
            print("falando carregando");	
        }
        #endregion
    }
}
