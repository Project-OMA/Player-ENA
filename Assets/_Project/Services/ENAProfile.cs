using UnityEngine;

namespace ENA.Services
{
    public class ENAProfile
    {
        #region Variables
        private string name;
        private int ID;
        #endregion
        #region Properties
        public int UserID => ID;
        public string UserName => name;
        #endregion
        #region Constructors
        public ENAProfile()
        {
            name = "Usuário Convidado";
            ID = -1;
        }

        public ENAProfile(string username, int id = -1)
        {
            name = username;
            ID = id;
        }
        #endregion
    }
}