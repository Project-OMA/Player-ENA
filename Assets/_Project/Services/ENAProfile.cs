using UnityEngine;

namespace ENA.Services
{
    public class ENAProfile
    {
        #region Properties
        [field: SerializeField] public int UserID {get; private set;}
        [field: SerializeField] public string UserName {get; private set;}
        #endregion
        #region Constructors
        public ENAProfile(string username = "Convidado", int id = -1)
        {
            UserName = username;
            UserID = id;
        }
        #endregion
    }
}