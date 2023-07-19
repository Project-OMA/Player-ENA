using System;
using System.Threading.Tasks;
using ENA.Provenance;
using ENA.Services;
using ENA.Utilities;
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
        [SerializeField] SessionTracker tracker;
        [Header("Displays")]
        [SerializeField] PauseMenuDisplay pauseMenuDisplay;
        [SerializeField] TrackerDisplay trackerDisplay;
        [SerializeField] GameObject fullMapDisplay;
        #endregion
        #region Properties
        [field: SerializeField] public GameFlag GameplayIsPaused {get; private set;}
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
            tracker.ClearSession();
            manager.Pop(pauseMenuDisplay);
            onQuit?.Invoke();
            await SaveSession();
            SceneManager.LoadSceneAsync(BuildIndex.MainMenu);
        }

        public void ResumeGameplay()
        {
            manager.Pop(pauseMenuDisplay);
            GameplayIsPaused.Set(true);
            onResume?.Invoke();
        }

        public void PauseGameplay()
        {
            manager.Push(pauseMenuDisplay);
            GameplayIsPaused.Set(false);
            onPause?.Invoke();
        }

        public async Task SaveSession()
        {
            var userProfile = settingsProfile.LoggedProfile;
            var recordingTime = DateTime.Now;
            var logContents = LogBuilder.MakeLog(tracker.Model);

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
            if (GameplayIsPaused.Value) ResumeGameplay();
            else PauseGameplay();
        }
        #endregion
    }
}