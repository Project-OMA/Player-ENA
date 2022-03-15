using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDoor : MonoBehaviour
{
    public static bool _aberto=false,_fechado=true;
    public int _velocidade;
    public Collider _colisorFisico;
    public Transform _door;
    public float _angulaAberto, _anguloFechado;
    public static PrefabDoor _prefabDoor;
    //configurações de abrir a porta
    private bool _pertoDaPorta,_abriuPorta;
    

    void Start()
    {
        _prefabDoor = new PrefabDoor();
    }

    void Update()
    {
        print(_pertoDaPorta);
        if (_pertoDaPorta)
        {
            if (Input.GetAxis("Fire1") > 0 && _abriuPorta==false)
            {
                if (_aberto)
                {
                    _aberto = false;
                    _fechado = true;
                    _abriuPorta = true;
                }
                else
                {
                    _aberto = true;
                    _fechado = false;
                    _abriuPorta = true;
                }
            }
            else
            {
                _abriuPorta = false;
            }
            if (_aberto)
            {
                _door.localEulerAngles = Vector3.Slerp(_door.localEulerAngles, new Vector3(_door.localEulerAngles.x, _angulaAberto, _door.localEulerAngles.z), _velocidade * Time.deltaTime);
                _colisorFisico.enabled = false;
            }
            else if (_fechado)
            {
                _door.localEulerAngles = Vector3.Slerp(_door.localEulerAngles, new Vector3(_door.localEulerAngles.x, _anguloFechado, _door.localEulerAngles.z), _velocidade * Time.deltaTime);
                if (_door.eulerAngles.y < 10)
                {
                    _colisorFisico.enabled = true;
                }
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _pertoDaPorta = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _pertoDaPorta = false;
        }
    }
}
