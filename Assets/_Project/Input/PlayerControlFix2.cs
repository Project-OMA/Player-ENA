using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Obriga o uso do CharacterController mesmo que a gameobject não tenha
[RequireComponent (typeof(CharacterController))]

public class PlayerControlFix2 : MonoBehaviour
{
	[Header ("Busca automaticamente o componente CharacterController")]
	//cria um atributo para o component CharacterController
	public CharacterController _characterController;
	//define os valores para pular,velocidade de correr e gravidade
	[Header ("Configuração de movimento e gravidade")]
	//public float _jumpSpeed;
	public float _moveSpeed;
	public float _distStep;
	public float _gravity;
	//para detectar se o joystick foi pressionado e liberar para não movimentar enquanto o valor for maior que zero
	private bool _stickPressed=false;

	//do outro script
	private AudioSource hitSound;
	//
	Quaternion _rotate=Quaternion.identity;
	public Vector3 _newPosition = Vector3.zero;
	public Vector3 _moveDirection = Vector3.zero;
	public bool _walking = false;
	public float _easyInOut=0;


	void Start ()
	{
		//pega o component CharacterController
		_characterController = GetComponent<CharacterController> ();

        //ajustando a distancia para a posição do player para poder zerar
        //_newPosition = transform.position;
        _easyInOut = 0;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		
		//mover para frente
		if (Input.GetAxis ("Vertical") > 0) {
            _easyInOut = 0;
            if (_stickPressed == false) {
                //_newPosition = new Vector3 (transform.position.x, transform.position.y, transform.localPosition.z + _distStep);
                _newPosition = transform.forward * _distStep;
                
                //o joystick foi pressionado
                _stickPressed = true;
			}
			//mover para tràs
		}else if (Input.GetAxis ("Vertical") < 0) {
            _easyInOut = 0;
            if (_stickPressed == false) {
                _newPosition = new Vector3 (transform.position.x, transform.position.y, transform.localPosition.z + _distStep);
                
            //o joystick foi pressionado
                _stickPressed = true;
			}
		}else {
			_stickPressed = false;
		}

		//girar para direita

		if (Input.GetAxis ("Horizontal") > 0) {
			_easyInOut = 0;
			if (_stickPressed == false) {
				_rotate.eulerAngles = new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
				_stickPressed = true;
			}
		} else {
			_easyInOut+=Time.deltaTime;
		}
		transform.rotation = Quaternion.Euler (Vector3.Lerp (transform.localRotation.eulerAngles,_rotate.eulerAngles,_easyInOut*Time.deltaTime));

		//definindo o movimento;
		if (_characterController.isGrounded) {

            

            transform.position = Vector3.Lerp(transform.TransformDirection(transform.position), _newPosition, Time.deltaTime);

            


            

		} else {
			//posicionar o player exatamente em cima do piso
			RaycastHit hit;
			Physics.Raycast (transform.position, transform.TransformDirection (Vector3.down), out hit, Mathf.Infinity);
			transform.position = new Vector3(hit.collider.gameObject.transform.position.x,transform.position.y,hit.collider.gameObject.transform.position.z);	
			_newPosition = transform.position;
		}
        _easyInOut += Time.deltaTime;
		//definindo gravidade
		_moveDirection.y=_moveDirection.y-(_gravity*Time.deltaTime);

		_characterController.Move (_moveDirection*Time.deltaTime);
	}

}
