using System;
using System.Collections;
using ENA.Services;
using UnityEngine;

namespace ENA.UI
{
    public class AuthCoordinator: UICoordinator
    {
        #region Constants
        const string LoginKey = "loginKey";
        #endregion
        #region Variables
        AuthService authService;
        [SerializeField] SettingsProfile profile;
        [Header("Panels")]
        [SerializeField] AuthDisplay authDisplay;
        [SerializeField] UIPanel signupPanel;
        [Header("Debug")]
        [SerializeField] bool guestMode;
        #endregion
        #region UICoordinator Implementation
        public override void Setup()
        {
            authService = manager.Get<AuthService>();
            WaitForLogin = new WaitUntil(authService.IsLogged);
        }
        #endregion
        #region Methods
        public void AskForAuthentication(Action<ENAProfile> completion)
        {
            if (authService.IsLogged()) return;

            StartCoroutine(RequestAuthentication(completion));
        }

        public void Authenticate()
        {
            var (login, password) = authDisplay.GetCredentials();
            Validate(login, password);
        }

        private string GetLoginKey() => PlayerPrefs.GetString(LoginKey);

        public async void Logout() => await authService.Logout();

        private async void Validate(string login, string password)
        {
            profile.LoggedProfile = await authService.LoginWith(login, password);
            manager.Pop(authDisplay);
        }
        #endregion
        #region Coroutines
        public WaitUntil WaitForLogin;
        IEnumerator RequestAuthentication(Action<ENAProfile> completion)
        {
            if (guestMode) {
                Validate("Convidado", "guest");
            } else {
                manager.Push(authDisplay);
                yield return WaitForLogin;
            }

            completion?.Invoke(authService.Profile);
        }
        #endregion
    }
}