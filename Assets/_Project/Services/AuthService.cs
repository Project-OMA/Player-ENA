using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ENA.Services
{
    public class AuthService: IService
    {
        #region Interfaces
        public interface CredentialManager
        {
            public Task<ENAProfile> ValidateCredentials(string login, string password);
            public Task<ENAProfile> ValidateToken(string userToken);
        }
        #endregion
        #region Variables
        ENAProfile loggedProfile;
        CredentialManager credentialManager;
        #endregion
        #region Properties
        public ENAProfile Profile => loggedProfile;
        #endregion
        #region Constructors
        public AuthService()
        {

        }
        #endregion
        #region Methods
        public bool IsLogged()
        {
            return loggedProfile != null;
        }

        public Task<bool> Logout()
        {
            loggedProfile = null;
            return Task.FromResult(true);
        }

        public async Task<ENAProfile> LoginWith(string login, string password)
        {
            loggedProfile = await credentialManager.ValidateCredentials(login, password);
            return loggedProfile;
        }

        public Task<ENAProfile> LoginWith(string loginToken)
        {
            return credentialManager.ValidateToken(loginToken);
        }

        public void SetCredentialManager(CredentialManager manager)
        {
            credentialManager = manager;
        }
        #endregion
    }
}