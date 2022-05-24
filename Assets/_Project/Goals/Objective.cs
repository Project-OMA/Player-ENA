using UnityEngine;

namespace ENA.Goals
{
    public class Objective
    {
        public AudioSource hitSound;
        public string nameTTS;
        public AudioSource audioRescueSound;
        public AudioSource sound;
        public string name;

        public Objective(string nameTTS, AudioSource sound,string name)
        {
            //this.hitSound = hitSound;
            this.nameTTS = nameTTS;
            this.sound = sound;
            this.name = name;
            //this.audioRescueSound = audioRescueSound;
        }
    }
}