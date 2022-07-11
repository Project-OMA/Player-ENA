using UnityEngine;

namespace ENA.Services
{
    public class ENAProfile
    {
        #region Variables
        private string name;
        private string ID;
        #endregion
        #region Properties
        public string UserID => ID;
        public string UserName => name;
        #endregion
        #region Constructors
        public ENAProfile()
        {
            name = "Test";
            ID = "0000";
        }

        public ENAProfile(string username)
        {
            name = username;
            ID = "0000";
        }
        #endregion
    }
}