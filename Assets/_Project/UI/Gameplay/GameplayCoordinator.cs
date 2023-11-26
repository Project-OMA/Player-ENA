using System;
using System.Threading.Tasks;
using ENA.Provenance;
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
        #region MonoBehaviour Lifecycle
        void OnDestroy()
        {
            UAP_AccessibilityManager.UnregisterOnPauseToggledCallback(PauseGameplay);
		    UAP_AccessibilityManager.UnregisterOnBackCallback(ResumeGameplay);
            SetAccessibilityActive(true);
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            UAP_AccessibilityManager.RegisterOnPauseToggledCallback(PauseGameplay);
            UAP_AccessibilityManager.RegisterOnBackCallback(ResumeGameplay);
            GameplayIsPaused.Set(false);
            SetAccessibilityActive(true);
        }
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            pauseMenuDisplay.gameObject.SetActive(false);
        }
        #endregion
        #region Methods
        public void SetAccessibilityActive(bool value)
        {
            UAP_AccessibilityManager.PauseAccessibility(!value);
        }

        public async void ReturnToMainMenu()
        {
            manager.Pop(pauseMenuDisplay);
            onQuit?.Invoke();
            await SaveSession();
            SceneManager.LoadSceneAsync(BuildIndex.MainMenu);
        }

        public void ResumeGameplay()
        {
            manager.Pop(pauseMenuDisplay);
            GameplayIsPaused.Set(false);
            onResume?.Invoke();
            SetAccessibilityActive(false);
        }

        public void PauseGameplay()
        {
            SetAccessibilityActive(true);
            manager.Push(pauseMenuDisplay);
            GameplayIsPaused.Set(true);
            onPause?.Invoke();
            UAP_AccessibilityManager.SelectElement(pauseMenuDisplay.StartLocation);
        }

        public async Task SaveSession()
        {
            var userProfile = settingsProfile.LoggedProfile;
            var recordingTime = DateTime.Now;
            var logContents = JsonUtility.ToJson(tracker.Model);

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