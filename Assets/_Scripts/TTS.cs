using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class TTS : MonoBehaviour {

    // Use this for initialization
	void Start () {
        EasyTTSUtil.Initialize(EasyTTSUtil.Brazil);

    }

    void OnApplicationQuit()
    {
        EasyTTSUtil.Stop();
    }

}
