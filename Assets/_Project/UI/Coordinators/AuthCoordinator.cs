using System;
using System.Collections;
using ENA.Input;
using ENA.Persistency;
using ENA.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ENA.UI
{
    public class AuthCoordinator: UICoordinator
    {
        #region Variables
        AuthService authService;
        [SerializeField] SettingsProfile profile;
        [Header("Panels")]
        [SerializeField] AuthDisplay authDisplay;
        [SerializeField] UIPanel signupPanel;
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
            if (!authService.IsLogged()) {
                StartCoroutine(RequestAuthentication(completion));
            }
        }

        public void Authenticate()
        {
            var credentials = authDisplay.GetCredentials();
            Validate(credentials.login, credentials.password);
        }

        private string GetLoginKey()
        {
            const string LoginKey = "loginKey";
            return PlayerPrefs.GetString(LoginKey);
        }

        public async void Logout()
        {
            await authService.Logout();
        }

        private async void Validate(string login, string password)
        {
            var user = await authService.LoginWith(login, password);
            if (await authService.LoginWith(login, password) is ENAProfile loggedUser) {
                profile.LoggedProfile = loggedUser;
            }
            manager.Pop(authDisplay);
        }
        #endregion
        #region Coroutines
        public WaitUntil WaitForLogin;
        IEnumerator RequestAuthentication(Action<ENAProfile> completion)
        {
            manager.Push(authDisplay);
            yield return WaitForLogin;
            completion?.Invoke(authService.Profile);
        }
        #endregion
    }
}