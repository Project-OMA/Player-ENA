using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CopiarTamanhoMiniMapa : MonoBehaviour {
	public RectTransform minimapa;
	
	void Start () {
		gameObject.GetComponent<RectTransform>().sizeDelta = minimapa.sizeDelta;
	}
}
