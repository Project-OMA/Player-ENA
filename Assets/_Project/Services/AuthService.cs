using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ENA.Services
{
    public class AuthService: IService
    {
        #region Interfaces
        public interface ICredentialManager
        {
            public Task<ENAProfile> ValidateCredentials(string login, string password);
            public Task<ENAProfile> ValidateToken(string userToken);
        }
        #endregion
        #region Variables
        ICredentialManager credentialManager;
        #endregion
        #region Properties
        [field: SerializeField] public ENAProfile Profile {get; private set;}
        #endregion
        #region Methods
        public bool IsLogged()
        {
            return Profile != null;
        }

        public Task<bool> Logout()
        {
            Profile = null;
            return Task.FromResult(true);
        }

        public async Task<ENAProfile> LoginWith(string login, string password)
        {
            Profile = await credentialManager.ValidateCredentials(login, password);
            return Profile;
        }

        public async Task<ENAProfile> LoginWith(string loginToken)
        {
            Profile = await credentialManager.ValidateToken(loginToken);
            return Profile;
        }

        public void SetCredentialManager(ICredentialManager manager)
        {
            credentialManager = manager;
        }
        #endregion
    }
}