using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Obriga o uso do CharacterController mesmo que a gameobject não tenha
[RequireComponent(typeof(CharacterController))]

public class PlayerControlFix : MonoBehaviour
{
    public int pontosCardeais,pontosCardPassados;
    public int contPassos,contRotacao;
    public float distPasso;
    public GameObject corpo;
    int frenteTraz;
    public bool andando;
    public float speed;
    public Transform target;
    public int rotacaoCamera;
    public int _atualRotation;
    [Header("Busca automaticamente o componente CharacterController")]
    //cria um atributo para o component CharacterController
    public CharacterController _characterController;
    //define os valores para pular,velocidade de correr e gravidade
    [Header("Configuração de movimento e gravidade")]
    //configurações do andar
    public float _moveSpeed;
    public float _distStep;
    public float _gravity;

    //configurações de abrir a porta
    private bool _pertoDaPorta;
    private bool _abriuAPorta = false;

    //
    //para detectar se o joystick foi pressionado e liberar para não movimentar enquanto o valor for maior que zero
    public bool _stickPressed = false;
    //configuração de som
    public bool _andando = false;
    private AudioSource hitSound;
    private AudioSource stepSound;
    public AudioSource _beepLeft, _beepRight;
    //
    public Quaternion _rotate = Quaternion.identity;
    int _direcao = 0;
    public Vector3 _newPosition = Vector3.zero;
    public Vector3 _moveDirection = Vector3.zero;

    public float _easyInOut = 0;

    public ObjetiveController objetiveController;

    public List<Vector3> vectorPositions;
    List<LineRenderer> tracks = new List<LineRenderer>();
    List<Vector3[]> positions = new List<Vector3[]>();

    float timeAux = 0;

    void Start()
    {
        pontosCardPassados = pontosCardeais;
        contPassos =0;
        contRotacao = -2;
        Invoke("PararDeAndar", 0.1f);
        //pega o component CharacterController
        _characterController = GetComponent<CharacterController>();
        _direcao = (int)transform.localEulerAngles.y;
        //ajustando a distancia para a posição do player para poder zerar
        //_newPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection.y = _moveDirection.y - (_gravity * Time.deltaTime);
        _easyInOut += Time.deltaTime;

        //indica a rotação atual do player

        if (ControleMenuPrincipal.giroscopioValue)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.transform.eulerAngles.y, transform.eulerAngles.z);
            AndarLivre();
        }
        else
        {
            AndarLivre();
            Rotacionar();
        }
        if (andando)
        {
            Andar();
        }
        QualDirecaoOlhando();
        VerificaRot();
    }

    public void updateVectorPositions()
    {
        vectorPositions.Add(transform.position);
    }
    void Rotacionar()
    {
        if (Input.GetAxis("Horizontal") > 0 && (int)transform.eulerAngles.y == (int)_rotate.eulerAngles.y && !andando)
        {
            _easyInOut = 0;
            if (_stickPressed == false)
            {
                // contRotacao++;
                _direcao += 90;
                _rotate.eulerAngles = new Vector3(transform.eulerAngles.x, _direcao, transform.eulerAngles.z);
                _stickPressed = true;
                BeepOnTurn(true, false);
            }
            //girar para esquerda
        }
        else if (Input.GetAxis("Horizontal") < 0 && (int)transform.eulerAngles.y == (int)_rotate.eulerAngles.y && !andando)
        {
            _easyInOut = 0;
            if (_stickPressed == false)
            {
                // contRotacao++;
                _direcao -= 90;
                _rotate.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, _direcao, transform.localRotation.eulerAngles.z);
                _stickPressed = true;
                BeepOnTurn(false, true);
            }
        }
        else
        {
            _stickPressed = false;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotate, _easyInOut * _moveSpeed * Time.deltaTime * 2);
    }
    void AndarLivre()
    {
        if (NewInput.GetAxisVerticalDown())
        {
            if (andando)
            {
                return;
            }
            else
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    frenteTraz = 1;
                }
                else
                {
                    frenteTraz = -1;
                }
                contPassos++;
                andando = true;
                Invoke("PararDeAndar", distPasso);
            }
        }
    }
    void PararDeAndar()
    {

        if (ControleMenuPrincipal.giroscopioValue)
        {
            andando = false;
            transform.localPosition = new Vector3(transform.localPosition.x, 0.64f, transform.localPosition.z);
        }
        else
        {
            andando = false;
            transform.position = corpo.transform.position;
        }
    }
    void Andar()
    {
        _characterController.Move(frenteTraz * transform.forward * Time.deltaTime * speed);
    }
    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (col.gameObject.tag == "floor")
        {
            corpo = col.gameObject.GetComponent<GetTarget>().target;
        }
        else
        {
            frenteTraz = -1;
            andando = true;
            Invoke("PararDeAndar", 0.1f);
        }
        if (col.gameObject.tag == "objects" || col.gameObject.tag == "objetives")
        {
            if (col.gameObject.tag == "objects")
            {
                UserModel.colisions++;
                //Aqui fica a vibração/vibration, sempre que ele colidir com qualquer objeto

                #if UNITY_IOS
                if(ControleMenuPrincipal.vibrationValue){
                    Handheld.Vibrate();
                }
                #endif
                //hitSound = col.gameObject.GetComponent<AudioSource>();
                //hitSound.Play();
                print(col.gameObject.name);

                ((objectCollider)col.gameObject.GetComponent<objectCollider>()).Collision();



                if (objetiveController.objetives.Count > 0 && (col.gameObject == (objetiveController.objetives[0])))
                {

                    objetiveController.StopObjectiveAudio();
                    objetiveController.PlayFindObjective();
                    objetiveController.objetives.RemoveAt(0);
                    UserModel.countQualTempo++;
                    if (objetiveController.objetives.Count != 0)
                        objetiveController.StartObjectiveAudio(5);


                    print("Sim");


                    LineRenderer track = col.gameObject.GetComponent<LineRenderer>();
                    List<Vector3> pos = vectorPositions;


                    pos.Add(col.transform.position);

                    positions.Add(pos.ToArray());
                    tracks.Add(track);
                    pos.Clear();

                    // getParcialTime();
                }

                else
                {
                    print("No");
                }

                //dá um passo para trás quando colidir
                //indica a rotação atual do player
                int _atualRotation = (int)transform.eulerAngles.y;
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (_atualRotation == 0)
                    {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z + _distStep);
                    }
                    if (_atualRotation == 90)
                    {
                        _newPosition = new Vector3(_newPosition.x + _distStep, 0, _newPosition.z);
                    }
                    if (_atualRotation == 180)
                    {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z - _distStep);
                    }
                    if (_atualRotation == 270)
                    {
                        _newPosition = new Vector3(_newPosition.x - _distStep, 0, _newPosition.z);
                    }
                }
                //
                if (Input.GetAxis("Vertical") > 0)
                {
                    if (_atualRotation == 0)
                    {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z - _distStep);
                    }
                    if (_atualRotation == 90)
                    {
                        _newPosition = new Vector3(_newPosition.x - _distStep, 0, _newPosition.z);
                    }
                    if (_atualRotation == 180)
                    {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z + _distStep);
                    }
                    if (_atualRotation == 270)
                    {
                        _newPosition = new Vector3(_newPosition.x + _distStep, 0, _newPosition.z);
                    }
                }
            }
        }
        else if (col.gameObject.tag == "floor")
        {
            stepSound = col.gameObject.GetComponent<AudioSource>();
            {
                if (_andando)
                {
                    stepSound.Play();
                }
            }

        }
    }

    public void updatePositions()
    {

        vectorPositions.Add(_characterController.transform.position);
    }

    // public void getParcialTime()
    // {

    //     if (objetiveController.stageCount == 0)
    //     {

    //         UserModel.parcialTime[0] = Time.time - UserModel.time;
    //         print("Parcial: " + UserModel.parcialTime[0]);
    //     }
    //     else
    //     {
    //         if (objetiveController.stageCount < objetiveController.objetives.Count)
    //         {
    //             UserModel.parcialTime[objetiveController.stageCount] = Time.time - timeAux;
    //             print("Parcial: " + UserModel.parcialTime[objetiveController.stageCount]);
    //         }

    //     }

    //     timeAux = Time.time;
    // }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        for (int i = 0; i < tracks.ToArray().Length; i++)
        {
            tracks.ToArray()[i].positionCount = positions.ToArray()[i].Length;
            tracks.ToArray()[i].SetPositions(positions.ToArray()[i]);


        }


        //UnityEngine.XR.XRSettings.enabled = false;
        //VRSettings.enabled = false;

        objetiveController.saveInfos();
        //SceneManager.LoadSceneAsync(0);


    }


    //soar o beep quando gira
    void BeepOnTurn(bool _right, bool _left)
    {

        if (_right)
        {
            if (_beepRight.isPlaying == false)
            {
                _beepRight.Play();
                BeepOnTurn(false, false);
            }

        }
        if (_left)
        {
            if (_beepLeft.isPlaying == false)
            {
                _beepLeft.Play();
                BeepOnTurn(false, false);
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "door")
        {
            _pertoDaPorta = true;
        }

        if (other.tag == "target")
        {
            corpo = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "door")
        {
            _pertoDaPorta = false;
        }
        // if(other.tag == "target"){
        //     corpo = null;
        // }
    }
    void QualDirecaoOlhando(){
        if(rotacaoCamera >= 315 && rotacaoCamera < 360){
            pontosCardeais = 1;
        }else if(rotacaoCamera >= 0 && rotacaoCamera < 45){
            pontosCardeais = 1;
        }else if(rotacaoCamera >= 45 && rotacaoCamera < 135){
            pontosCardeais = 2;
        }else if(rotacaoCamera >= 135 && rotacaoCamera < 225){
            pontosCardeais = 3;
        }else if(rotacaoCamera >= 225 && rotacaoCamera < 315){
            pontosCardeais = 4;
        }
    }
    void VerificaRot(){
        if(pontosCardPassados != pontosCardeais){
            contRotacao++;
        }
        pontosCardPassados = pontosCardeais;
    }
}