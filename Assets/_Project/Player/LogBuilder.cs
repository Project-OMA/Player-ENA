using System;
using ENA.Input;
using ENA.Services;
using ENA.TTS;
using ENA.Utilities;
using UnityEngine;

namespace ENA.Player
{
    public class LogBuilder
    {
        #region Static Methods
        public static string MakeLog(ENAProfile profile, DateTime time, PlayerController playerController)
        {
            string timeString = $"{time.Hour}_{time.Minute}";
            var userName = (profile?.UserName) + "-" + timeString;
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
            yaml.Mapping("NumeroDeRotações", playerController.RotationCount.ToString());
            yaml.Mapping("NumeroDePassos", playerController.StepCount.ToString());
            yaml.Mapping("Ajudas", null);
            using (var indent = yaml.Indent()) {
                yaml.Mapping("Objetivo", InitAudios.numberOfTipsGiven.ToString());
                yaml.Mapping("PontoInicial", DirecaoInicial.ajudaInicial.ToString());
            }

            var contents = yaml.Output();
            return contents;
        }
        #endregion
    }
}