using System;
using UnityEngine;

namespace MicelioUnity
{
    [Serializable]
    public class Device
    {
        public string device_id;
        public string model;
        public int screen_width;
        public int screen_height;
        public string system_name;

        public Device()
        {
            device_id = SystemInfo.deviceUniqueIdentifier;
            model = SystemInfo.deviceModel;
            screen_width = Screen.width;
            screen_height = Screen.height;
            system_name = SystemInfo.operatingSystem;
        }

		/**
		<summary>Checks if any data was not recognized.</summary>
		*/
        public bool VerifyDataIntegrity()
        {
			return screen_height > 0 && screen_width > 0 && device_id != null && model != null && system_name != null;
        }
    }
}