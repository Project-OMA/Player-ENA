using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ENA.Input
{
    public class ControleMenuPrincipal: MonoBehaviour
    {
        #region Voice Lines
        public string LoadingMessage {get; set;}
        #endregion
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

        public void ToggleVRGlasses(bool value)
        {
            Debug.Log($"VR Glasses? {value}");
            oculosValue = value;
        }

        public void ToggleGyroscope(bool value)
        {
            Debug.Log($"Gyroscope? {value}");
            giroscopioValue = value;
        }

        public void ToggleDisappearingElements(bool value)
        {
            Debug.Log($"Disappearing Elements? {value}");
            elementosValue = value;
        }

        public void ToggleMinimap(bool value)
        {
            Debug.Log($"Mini Map? {value}");
            mapaValue = value;
        }

        public void ToggleVibration(bool value)
        {
            Debug.Log($"Vibration? {value}");
            vibrationValue = value;
        }

        public void FalarCarregando()
        {
            UAP_AccessibilityManager.Say(LoadingMessage, false);
            print("falando carregando");	
        }
        #endregion
    }
}
