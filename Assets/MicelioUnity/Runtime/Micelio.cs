using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MicelioUnity
{
    public class Micelio
    {
        private string defaultURL = "https://cursa.eic.cefet-rj.br/micelio/api";
        private string token;
        private string deviceID;
        private bool isDevEnvironment;
        private string configFilePath = Application.persistentDataPath + "/micelioDeviceSettings.bin";

        //construtor classe Micelio
        public Micelio(string token, string environment = "")
        {
            this.token = token;
            deviceID = GetDeviceInformation();
            isDevEnvironment = environment == "dev";
        }

        /**
        <summary>Serializes the object to a JSON format.</summary>
        */
        public static string ToJSON(object o)
        {
            return JsonUtility.ToJson(o);
        }
        /**
        <summary>Returns the ID of the used device.</summary>
        */
        private string GetDeviceInformation()
        {
            Device device;

            //carrega informações do device, se existir
            if (File.Exists(this.configFilePath))
            {
                BinaryFormatter formatter = new();
                using (FileStream fs = new(configFilePath, FileMode.Open)) {
                    object deserialized = formatter.Deserialize(fs);
                    device = (Device)deserialized;
                }
                Log("Device information was successfully found!");
            }
            else
            {
                Log("Device information not found! Loading data to generate the file.");
                device = new Device();

                //teste se todos os campos de Device estão completos
                if (device.VerifyDataIntegrity())
                {
                    //envia cadastro do dispositivo e guardando os dados
                    SendDevice(device);
                }
            }
            return device.device_id;
        }
        /**
        <summary>Sends device information to the Micelio Database.</summary>
        */
        private void SendDevice(Device device)
        {
            string payload = ToJSON(device);

            //verifica sucesso do cadastro, e cria arquivo de configuração
            if (SendAPIRequest("/device", "POST", payload, "The device data was successfully sent."))
            {
                BinaryFormatter formatter = new();

                using (FileStream fs = new(configFilePath, FileMode.Create))
                {
                    formatter.Serialize(fs, device);
                }

                Log("Device information was successfully registered!");
            }
        }
        /**
        <summary>Sends session information to the Micelio Database.</summary>
        */
        public void StartSession(Session session)
        {
            string payload = ToJSON(session);
            SendAPIRequest("/session", "POST", payload, "The session was successfully started.");
        }
        /**
        <summary>Sends Activity information to the Micelio Database.</summary>
        */
        public string SendActivity(Activity activity)
        {
            string payload = ToJSON(activity);
            SendAPIRequest("/activity", "POST", payload, "The activity data was successfully sent.");
            return activity.activity_id;
        }
        /**
        <summary>Sends all required information for closing a session to the Micelio Database.</summary>
        */
        public void CloseSession()
        {
            DateTime currentTime = DateTime.Now;
            string payload = "{\"end_time\" : \"" + currentTime.ToString("HH:mm:ss") + "\"}";
            SendAPIRequest("/session", "PUT", payload, "The session was successfully ended.");
        }

        //função para envio de dados a API
        /**
        <summary>Sends data to the API</summary>
        <returns>True when the operation is successful. False when the request fails.</returns>
        */
        private bool SendAPIRequest(string endPoint,
                                    string method,
                                    string payload,
                                    string successLogMessage = "The information was successfully sent.",
                                    string errorLogMessage = "Cannot send data to Micelio!")
        {
            bool success = true;
            try {
                string baseURL = defaultURL + endPoint;

                //criação da requisição e configuração dos parametros
                if (isDevEnvironment)
                {
                    baseURL += "/test";
                }

                var webRequest = WebRequest.CreateHttp(baseURL);
                webRequest.ContentType = "application/json";
                webRequest.UserAgent = "MicelioUnityAgent";
                webRequest.Headers.Add("token", token);
                webRequest.Headers.Add("device_id", deviceID);
                webRequest.ContentLength = payload.Length;
                webRequest.Method = method;

                //envio do payload para a API
                using (var stream = new StreamWriter(webRequest.GetRequestStream()))
                {
                    stream.Write(payload);
                }

                //recebimento da resposta da API
                using (var response = webRequest.GetResponse())
                {
                    //montando a stream de resposta
                    var dataStream = response.GetResponseStream();
                    StreamReader reader = new(dataStream);

                    //status code da requisição
                    HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;

                    //resposta do servidor
                    object responseObject = reader.ReadToEnd();

                    dataStream.Close();
                }
                Log(successLogMessage + "\n" + payload + "\nRequest -> [" + method + "]: " + endPoint);
            }
            catch (Exception e)
            {
                success = false;
                Log(errorLogMessage + " \n" + e);
            }

            return success;
        }
        #region Static Methods
        public static void Log(string message)
        {
            Debug.Log("[MICELIO LOG] "+message);
        }
        #endregion
    }
}