using ENA.Metrics;
using UnityEngine;

namespace ENA.Services
{
    public class MicelioWebService
    {
        #region Variables
        bool enabled;
        Micelio micelio;
        Session session;
        #endregion
        #region Classes
        public MicelioWebService(string apiToken, bool devMode, bool enabled)
        {
            micelio = new Micelio(apiToken, devMode ? "dev" : string.Empty);
            enabled = false;
        }
        #endregion
        #region Methods
        public void Disable()
        {
            if (!enabled) return;

            micelio.CloseSession();
            enabled = false;
        }

        public void OpenSession(string sessionName, string groupID, string levelID)
        {
            if (!enabled) return;

            session = new Session(Application.systemLanguage.ToString(), levelID);
            session.SetName(sessionName);
            session.SetSessionGroup(groupID);

            micelio.StartSession(session);
        }

        public void Register(Activity activity)
        {
            if (!enabled) return;

            activity.SendTo(micelio);
        }
        #endregion
    }
}