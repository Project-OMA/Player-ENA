using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ENA.Maps;
using ENA.Utilities;
using UnityEngine;

namespace ENA.Services
{
    public partial class LocalCache
    {
        #region Constants
        public const string LogsFolder = "logs/";
        #endregion
        #region Properties
        private static string LogsFullPath => DataPath.Persistent+LogsFolder;
        #endregion
        #region Methods
        public void VerifyLogsFolder()
        {
            var logsPath = LogsFullPath;
            if (!Directory.Exists(logsPath)) {
                Directory.CreateDirectory(logsPath);
            }
        }
        #endregion
        #region Static Methods
        private static string GenerateSessionTag(DateTime recordingTime, ENAProfile profile)
        {
            string day = recordingTime.Date.ToString().Split(' ')[0];
            day = day.Replace("/", "-");

            return $"{profile.UserName}_{day}_{recordingTime.Hour}_{recordingTime.Minute}";
        }

        public static async Task SaveLog(DateTime recordingTime, ENAProfile profile, string contents)
        {
            string sessionTag = GenerateSessionTag(recordingTime, profile);
            string path = LogsFullPath+$"{sessionTag}_Log.txt";

            using (var fileWriter = new StreamWriter(path, true)) {
                await fileWriter.WriteAsync(contents);
            }

            Debug.Log($"Saved Log to Path: {path}");
        }

        public static void SaveTracker(DateTime recordingTime, ENAProfile profile, RenderTexture texture)
        {
            string sessionTag = GenerateSessionTag(recordingTime, profile);
            string path = LogsFullPath+$"{sessionTag}_Tracker.png";

            texture.OutputToFile(path);

            Debug.Log($"Saved Screenshot to Path: {path}");
        }
        #endregion
    }
}