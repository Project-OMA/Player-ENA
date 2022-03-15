using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ObjectsNames
{
    public string GameName;
    public string StringTSS_PT;
    public string StringTSS_IN;
    public string StringTSS_ES;
}

public class ObjetivesDictionary : ScriptableObject
{
    [Header("Objetos Interativos")]
    public ObjectsNames[] objectNames;


    public string retornaObjectName(string gamename)
    {
        foreach (ObjectsNames obj in objectNames)
        {
            if (string.Equals(gamename, obj.GameName))
            {
                if (Application.systemLanguage == SystemLanguage.Spanish)
                {
                    //Outputs into console that the system is Portuguese
                    return obj.StringTSS_ES;

                }
                else if (Application.systemLanguage == SystemLanguage.English)
                {
                    return obj.StringTSS_IN;
                    
                }
                else
                {
                    return obj.StringTSS_PT;
                    
                }             
            }
            
        }

        return "";
    }

}
