using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_FieldView : MonoBehaviour
{

    public Slider _slider;
    public Camera _camera;
    
    // Update is called once per frame
    void Update()
    {
        _camera.fieldOfView = _slider.value;
    }
}
