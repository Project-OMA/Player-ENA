using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Obriga o uso do CharacterController mesmo que a gameobject não tenha
[RequireComponent (typeof(CharacterController))]

public class PlayerControl : MonoBehaviour
{
	[Header ("Busca automaticamente o componente CharacterController")]
	//cria um atributo para o component CharacterController
	public CharacterController _characterController;
	//define os valores para pular,velocidade de correr e gravidade
	[Header ("Configuração da velocidade e gravidade")]
	//public float _jumpSpeed;
	public float _moveSpeed;
	public float _gravity;
	//usar para quando o player bater na parede
	[Header ("velocidade da reação quando bate na parede")]
	public float _tempoParado;
	public float _andarParaTras;
	private bool _livrePraAndar;
	//utilizado para gravidade
	private Vector3 _gravityDirection = Vector3.zero;
	//pegar referencia de rotação da câmera
	private GameObject _camera;
	//Som dos passos
	[Header ("Som dos passos")]
	public float _distanciaPasso;
	private AudioSource stepSound;
	//do outro script
	private AudioSource hitSound;
	//medindo distancia dos passos
	float _distanciaPercorrida=0;
	Vector3 _lastPosition = Vector3.zero;
	//comandos temporários só para gerar o apk para o Agerbson
	public Slider _sliderPlayer, _sliderParede, _sliderPassos;


	void Start ()
	{
		//
		_livrePraAndar = true;
		//pega o component CharacterController
		_characterController = GetComponent<CharacterController> ();
		//Pegar o objeto camera
		_camera = GetComponentInChildren<Camera> ().gameObject;
		//pegar os sliders
		_sliderPlayer = GameObject.Find ("SliderPlayer").GetComponent<Slider> ();
		_sliderParede = GameObject.Find ("SliderParede").GetComponent<Slider> ();
		_sliderPassos = GameObject.Find ("SliderPassos").GetComponent<Slider> ();
		//ajustando a distancia para a posição do player para poder zerar
		_lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
        
		//definindo valores do slider //pode ser retirado depois
		_moveSpeed = _sliderPlayer.value;
		_andarParaTras = _sliderParede.value;
		_distanciaPasso = _sliderPassos.value;

		//definindo a gravidade
		_gravityDirection.y -= _gravity * Time.deltaTime;
		//definindo o movimento;
		if (_livrePraAndar) {
			Vector3 _forward = transform.TransformDirection (_camera.transform.forward);
			Vector3 _side = transform.TransformDirection (_camera.transform.right);
			float _curSpeed = _moveSpeed * Input.GetAxis ("Vertical");
			float _sideSpeed = _moveSpeed * Input.GetAxis ("Horizontal");
			_characterController.SimpleMove (_side * _sideSpeed);
			_characterController.SimpleMove (_forward * _curSpeed);
		}

	}

	//aproveitado do outro script
	private void OnGUI ()
	{
		//GUI.Label (new Rect (0, 0, 500, 500), "Tempo: " + Time.time);
	}
	//do outro script
	void OnControllerColliderHit (ControllerColliderHit col)
	{

		if (col.gameObject.tag == "objects" || col.gameObject.tag == "objetives") {
			//joga o personagem para trás quando bate na parede
			Vector3 _forward = transform.TransformDirection (_camera.transform.forward);
			float _curSpeed = -_andarParaTras * Input.GetAxis ("Vertical");
			_characterController.SimpleMove (_forward * _curSpeed);
			//
			if (col.gameObject.tag == "objects") {
				UserModel.colisions++;
				hitSound = col.gameObject.GetComponent<AudioSource> ();
				hitSound.Play ();
				_livrePraAndar = false;
				StartCoroutine ("EsperePraAndar");
			}
		} else if (col.gameObject.tag == "floor") {
			//tocar o som dos passos de acordo com a distancia de um passo
			//calculando a distância percorrida
			_distanciaPercorrida += Vector3.Distance (transform.position, _lastPosition);
			_lastPosition = transform.position;
			print (_distanciaPercorrida);

			if (_distanciaPercorrida > _distanciaPasso) {
				stepSound = col.gameObject.GetComponent<AudioSource> ();
				stepSound.Play ();
				_distanciaPercorrida = 0;
			}
		}
	}

	//quando o player bater em algum objeto determina ele ficar parado por algum tempo
	IEnumerator EsperePraAndar ()
	{
		yield return new WaitForSeconds (_tempoParado);
		_livrePraAndar = true;
	}
}
