using System;

namespace MicelioUnity
{
    [Serializable]
    public class Session
    {
        public string name;
        public string language;
        public string game_stage;
        public string date;
        public string session_group;
        public string start_time;
        public string end_time;

        public Session(string language, string game_stage)
        {
            DateTime currentTime = DateTime.Now;
            this.language = language;
            this.game_stage = game_stage;
            date = currentTime.ToString("yyyy-MM-dd");
            start_time = currentTime.ToString("HH:mm:ss");
        }

        public void SetSessionGroup(string session_group)
        {
            this.session_group = session_group;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}