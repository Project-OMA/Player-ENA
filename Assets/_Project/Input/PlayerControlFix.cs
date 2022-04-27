using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ENA.Input
{
    //Obriga o uso do CharacterController mesmo que a gameobject não tenha
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControlFix : MonoBehaviour
    {
        #region Variables
        [SerializeField] int pontosCardeais;
        [SerializeField] int pontosCardPassados;
        [SerializeField] int contPassos;
        [SerializeField] int contRotacao;
        [SerializeField] float distPasso;
        [SerializeField] GameObject corpo;
        int frenteTraz;
        [SerializeField] bool andando;
        [SerializeField] float speed;
        [SerializeField] Transform target;
        [SerializeField] int rotacaoCamera;
        [Header("Busca automaticamente o componente CharacterController")]
        //cria um atributo para o component CharacterController
        [SerializeField] CharacterController _characterController;
        //define os valores para pular,velocidade de correr e gravidade
        [Header("Configuração de movimento e gravidade")]
        //configurações do andar
        [SerializeField] float _moveSpeed;
        [SerializeField] float _distStep;
        [SerializeField] float _gravity;

        //configurações de abrir a porta
        [SerializeField] bool _pertoDaPorta;

        //para detectar se o joystick foi pressionado e liberar para não movimentar enquanto o valor for maior que zero
        [SerializeField] bool _stickPressed = false;
        //configuração de som
        [SerializeField] bool _andando = false;
        [SerializeField] AudioSource stepSound;
        [SerializeField] AudioSource _beepLeft;
        [SerializeField] AudioSource _beepRight;

        [SerializeField] Quaternion _rotate = Quaternion.identity;
        int _direcao = 0;
        [SerializeField] Vector3 _newPosition = Vector3.zero;
        [SerializeField] Vector3 _moveDirection = Vector3.zero;

        [SerializeField] float _easyInOut = 0;

        [SerializeField] ObjetiveController objetiveController;

        [SerializeField] List<Vector3> vectorPositions;
        List<LineRenderer> tracks = new List<LineRenderer>();
        List<Vector3[]> positions = new List<Vector3[]>();
        #endregion
        #region Properties (Temp.)
        [Obsolete]
        public int RotationCount {
            get => contRotacao;
            set => contRotacao = value;
        }

        [Obsolete]
        public int StepCount {
            get => contPassos;
            set => contPassos = value;
        }

        [Obsolete]
        public Transform Target {
            get => target;
            set => target = value;
        }

        [Obsolete]
        public int CameraRotation {
            get => rotacaoCamera;
            set => rotacaoCamera = value;
        }

        [Obsolete]
        public Quaternion Rotate {
            get => _rotate;
            set => _rotate = value;
        }
        #endregion
        #region Methods
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            pontosCardPassados = pontosCardeais;
            contPassos = 0;
            contRotacao = -2;
            _direcao = (int)transform.localEulerAngles.y;

            Invoke(nameof(PararDeAndar), 0.1f);
        }

        private void Update()
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
            _easyInOut += Time.deltaTime;

            //indica a rotação atual do player

            if (ControleMenuPrincipal.giroscopioValue) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.transform.eulerAngles.y, transform.eulerAngles.z);
                AndarLivre();
            } else {
                AndarLivre();
                Rotacionar();
            }

            if (andando) {
                Andar();
            }

            QualDirecaoOlhando();
            VerificaRot();
        }

        public void updateVectorPositions()
        {
            vectorPositions.Add(transform.position);
        }

        private void Rotacionar()
        {
            bool lookingAtSameAngle = (int)transform.eulerAngles.y == (int)_rotate.eulerAngles.y && !andando;
            float axisValue = UnityEngine.Input.GetAxis("Horizontal");

            if (lookingAtSameAngle && axisValue != 0) {
                _easyInOut = 0;
                if (!_stickPressed) {
                    bool right = axisValue > 0;
                    if (right) {
                        _direcao += 90;
                        _rotate.eulerAngles = new Vector3(transform.eulerAngles.x, _direcao, transform.eulerAngles.z);
                    } else {
                        _direcao -= 90;
                        _rotate.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, _direcao, transform.localRotation.eulerAngles.z);
                    }
                    _stickPressed = true;
                    BeepOnTurn(right, !right);
                }
            } else {
                _stickPressed = false;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, _rotate, _easyInOut * _moveSpeed * Time.deltaTime * 2);
        }

        private void AndarLivre()
        {
            if (andando) return;

            if (AxisTracker.VerticalDown()) {
                frenteTraz = UnityEngine.Input.GetAxis("Vertical") > 0 ? 1 : -1;
                contPassos++;
                andando = true;
                Invoke(nameof(PararDeAndar), distPasso);
            }
        }

        private void PararDeAndar()
        {
            andando = false;
            if (ControleMenuPrincipal.giroscopioValue) {
                transform.localPosition = new Vector3(transform.localPosition.x, 0.64f, transform.localPosition.z);
            } else {
                transform.position = corpo.transform.position;
            }
        }

        private void Andar()
        {
            _characterController.Move(frenteTraz * transform.forward * Time.deltaTime * speed);
        }

        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            if (col.gameObject.tag == "floor") {
                corpo = col.gameObject.GetComponent<GetTarget>().target;
            } else {
                frenteTraz = -1;
                andando = true;
                Invoke(nameof(PararDeAndar), 0.1f);
            }

            if (col.gameObject.tag == "objects") {
                UserModel.colisions++;
                //Aqui fica a vibração/vibration, sempre que ele colidir com qualquer objeto

                #if UNITY_IOS
                if(ControleMenuPrincipal.vibrationValue)
                {
                    Handheld.Vibrate();
                }
                #endif
                print(col.gameObject.name);

                col.gameObject.GetComponent<objectCollider>().Collision();

                if (objetiveController.objetives.Count > 0 && (col.gameObject == (objetiveController.objetives[0]))) {
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
                } else {
                    print("No");
                }

                int _atualRotation = (int)transform.eulerAngles.y;
                float verticalAxis = UnityEngine.Input.GetAxis("Vertical");
                if (verticalAxis < 0) {
                    if (_atualRotation == 0) {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z + _distStep);
                    }
                    if (_atualRotation == 90) {
                        _newPosition = new Vector3(_newPosition.x + _distStep, 0, _newPosition.z);
                    }
                    if (_atualRotation == 180) {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z - _distStep);
                    }
                    if (_atualRotation == 270) {
                        _newPosition = new Vector3(_newPosition.x - _distStep, 0, _newPosition.z);
                    }
                }

                if (verticalAxis > 0) {
                    if (_atualRotation == 0) {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z - _distStep);
                    }
                    if (_atualRotation == 90) {
                        _newPosition = new Vector3(_newPosition.x - _distStep, 0, _newPosition.z);
                    }
                    if (_atualRotation == 180) {
                        _newPosition = new Vector3(_newPosition.x, 0, _newPosition.z + _distStep);
                    }
                    if (_atualRotation == 270) {
                        _newPosition = new Vector3(_newPosition.x + _distStep, 0, _newPosition.z);
                    }
                }
            } else if (col.gameObject.tag == "floor") {
                stepSound = col.gameObject.GetComponent<AudioSource>();
                if (_andando) {
                    stepSound.Play();
                }
            }
        }

        public void updatePositions()
        {
            vectorPositions.Add(_characterController.transform.position);
        }

        private void BeepOnTurn(bool _right, bool _left)
        {
            if (_right) {
                if (!_beepRight.isPlaying) {
                    _beepRight.Play();
                    BeepOnTurn(false, false);
                }
            }

            if (_left) {
                if (!_beepLeft.isPlaying) {
                    _beepLeft.Play();
                    BeepOnTurn(false, false);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "door") {
                _pertoDaPorta = true;
            }

            if (other.tag == "target") {
                corpo = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "door") {
                _pertoDaPorta = false;
            }
        }

        private void QualDirecaoOlhando()
        {
            if(rotacaoCamera >= 315 && rotacaoCamera < 360) {
                pontosCardeais = 1;
            } else if(rotacaoCamera >= 0 && rotacaoCamera < 45) {
                pontosCardeais = 1;
            } else if(rotacaoCamera >= 45 && rotacaoCamera < 135) {
                pontosCardeais = 2;
            } else if(rotacaoCamera >= 135 && rotacaoCamera < 225) {
                pontosCardeais = 3;
            } else if(rotacaoCamera >= 225 && rotacaoCamera < 315) {
                pontosCardeais = 4;
            }
        }

        private void VerificaRot()
        {
            if(pontosCardPassados != pontosCardeais) {
                contRotacao++;
            }

            pontosCardPassados = pontosCardeais;
        }

        public void ToggleControls()
        {
            this.enabled = !this.enabled;
        }
        #endregion
        #region Coroutines
        IEnumerator EndGame()
        {
            yield return new WaitForSeconds(5);
            LineRenderer[] lineRenderers = tracks.ToArray();

            for (int i = 0; i < lineRenderers.Length; i++) {
                Vector3[] linePoints = positions.ToArray()[i];
                lineRenderers[i].positionCount = linePoints.Length;
                lineRenderers[i].SetPositions(linePoints);
            }

            objetiveController.saveInfos();
        }
        #endregion
    }
}
