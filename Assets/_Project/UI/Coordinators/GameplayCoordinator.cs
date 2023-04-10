using System;
using System.Threading.Tasks;
using ENA.Input;
using ENA.Metrics;
using ENA.Persistency;
using ENA.Player;
using ENA.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ENA.UI
{
    public class GameplayCoordinator: UICoordinator
    {
        #region Variables
        [SerializeField] SettingsProfile settingsProfile;
        [SerializeField] RenderTexture minimap;
        [SerializeField] PlayerController playerController;
        [SerializeField] SessionTracker tracker;
        [Header("Displays")]
        [SerializeField] PauseMenuDisplay pauseMenuDisplay;
        [SerializeField] TrackerDisplay trackerDisplay;
        [SerializeField] GameObject fullMapDisplay;
        #endregion
        #region Properties
        public bool GameplayIsPaused => !playerController.enabled;
        #endregion
        #region Events
        [SerializeField] UnityEvent onPause;
        [SerializeField] UnityEvent onResume;
        [SerializeField] UnityEvent onQuit;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            pauseMenuDisplay.gameObject.SetActive(false);

            UAP_AccessibilityManager.RegisterOnPauseToggledCallback(PauseGameplay);
            UAP_AccessibilityManager.RegisterOnBackCallback(ResumeGameplay);
        }
        #endregion
        #region Methods
        public void OnDestroy()
        {
            UAP_AccessibilityManager.UnregisterOnPauseToggledCallback(PauseGameplay);
		    UAP_AccessibilityManager.UnregisterOnBackCallback(ResumeGameplay);
        }

        public async void ReturnToMainMenu()
        {
            manager.Pop(pauseMenuDisplay);
            onQuit?.Invoke();
            await SaveSession();
            tracker.ClearSession();
            SceneManager.LoadSceneAsync(BuildIndex.MainMenu);
        }

        public void ResumeGameplay()
        {
            manager.Pop(pauseMenuDisplay);
            playerController.enabled = true;
            onResume?.Invoke();
        }

        public void PauseGameplay()
        {
            manager.Push(pauseMenuDisplay);
            playerController.enabled = false;
            onPause?.Invoke();
        }

        public async Task SaveSession()
        {
            var userProfile = settingsProfile.LoggedProfile;
            var recordingTime = DateTime.Now;
            var logContents = LogBuilder.MakeLog(userProfile, recordingTime, tracker.Model);

            ShowTracker(recordingTime);
            LocalCache.SaveTracker(recordingTime, userProfile, minimap);
            await LocalCache.SaveLog(recordingTime, userProfile, logContents);

            while (UAP_AccessibilityManager.IsSpeaking()) {
                await Task.Delay(1000);
            }
        }

        public void ShowTracker(DateTime timestamp)
        {
            trackerDisplay.SetAnnotation(timestamp.ToString("MM/dd/yyyy h:mm"));
            manager.Push(trackerDisplay);
            fullMapDisplay.SetActive(true);
        }

        public void TogglePause()
        {
            if (GameplayIsPaused) ResumeGameplay();
            else PauseGameplay();
        }
        #endregion
    }
}