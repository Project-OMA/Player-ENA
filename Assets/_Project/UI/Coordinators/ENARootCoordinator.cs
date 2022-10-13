using ENA.Maps;
using ENA.Services;
using UnityEngine;

namespace ENA.UI
{
    [RequireComponent(typeof(AuthCoordinator))]
    [RequireComponent(typeof(MapDataCoordinator))]
    [RequireComponent(typeof(SettingsCoordinator))]
    public class ENARootCoordinator: UICoordinator
    {
        #region Variables
        [Header("Coordinators")]
        [SerializeField] AuthCoordinator authCoordinator;
        [SerializeField] MapDataCoordinator mapDataCoordinator;
        [SerializeField] SettingsCoordinator settingsCoordinator;
        [Header("Displays")]
        [SerializeField] MainMenuDisplay mainMenuDisplay;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            authCoordinator.Setup(manager);
            mapDataCoordinator.Setup(manager);
            settingsCoordinator.Setup(manager);

            AppInit();
        }
        #endregion
        #region Methods
        void AppInit()
        {
            var enaService = new ENAWebService();

            manager.Get<MapService>().SetDataSource(enaService);
            var authService = manager.Get<AuthService>();
            authService.SetCredentialManager(enaService);

            manager.Push(mainMenuDisplay);

            authCoordinator.AskForAuthentication(completion: (profile) => {
                mapDataCoordinator.SyncData(profile);
                mainMenuDisplay.SetHeader(profile.UserName);
            });
        }

        void Reset()
        {
            authCoordinator = GetComponent<AuthCoordinator>();
            mapDataCoordinator = GetComponent<MapDataCoordinator>();
            settingsCoordinator = GetComponent<SettingsCoordinator>();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        #endregion
    }
}