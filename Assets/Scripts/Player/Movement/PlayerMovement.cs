using UnityEngine;
using Mirror;


public class PlayerMovement : NetworkBehaviour
{
а а #region Movimento
а а [Header("Move")]
    public float MoveX; //Movimento no Eixo X
а а public float MoveY; //Movimento no Eixo Y
а а public bool isRun; //Estс correndo?
а а public float MoveSpd; //Velocidade atual de movimento
а а public float WalkSpd; //Velocidade de andar
а а public float RunSpd; //Velocidade de correr
а а public bool CanMove; //Pode andar?
а а #endregion
а а #region Pulo
а а [Header("Jump")]
    public float JumpForce; //Forчa de pulo
а а #endregion
а а #region Chуo
а а [Header("GroundCheck")]
    public bool isGround; //Estс no chуo?
а а public float GroundDis = 0.2f; //Distancia do Player ao chуo
а а public Transform GroundCheck; //Obj que verfica como chуo
а а public LayerMask GroundLayer; //Layer do chуo
а а #endregion
а а #region Componentes
а а Rigidbody rig; //Rigidbody
а а #endregion

а а void Awake() //Ao acordar
а а {
        rig = GetComponent<Rigidbody>();
    }

    public override void OnStartLocalPlayer() //Ao iniciar como player local
а а {
        if (!isLocalPlayer) return; //Se nуo for player local, retorna
а а }

    void Update()
    {
        if (!isLocalPlayer) return; //Se nуo for player local, retorna
а а а а #region Chama "Pular"
а а а а if (CanMove) //Se for possivel andar executa
а а а а {
            Jump(); //Executa funчуo de pulo
а а а а }
        #endregion

    }

    void FixedUpdate()
    {
        CanMove = !InventoryUI.isOpen;

        if (!CanMove) return; //Se nуo for possivel andar retorna

а а а а if (!isLocalPlayer) return; //Se nуo for player local, retorna
а а а а MoveX = Input.GetAxis("Horizontal"); //Teclas de movimento horizontal
а а а а MoveY = Input.GetAxis("Vertical"); //Teclas de movimento vertical

а а а а //Muda velocidade se correr/correr
а а а а if (Input.GetKey(KeyCode.LeftShift)) MoveSpd = RunSpd; else MoveSpd = WalkSpd;

а а а а Move();
    }


    #region Movimento
    void Move()
    {
        Vector3 direction = new Vector3(MoveX, 0, MoveY).normalized;
        Vector3 velocity = transform.TransformDirection(direction) * MoveSpd;

        rig.linearVelocity = new Vector3(velocity.x, rig.linearVelocity.y, velocity.z);
    }
а а #endregion

а а #region Pulo
а а void Jump() //Pulo
а а {
а а а а //Verifica se estс no chуo
а а а а isGround = Physics.CheckSphere(GroundCheck.position, GroundDis, GroundLayer);

        //Se clicar no botуo de pulo, e estar no chуo
        if (Input.GetButtonDown("Jump") && isGround)
        {
а а а а а а //Adiciona uma forчa ao rigidbody para cima
а а а а а а rig.linearVelocity = new Vector3(rig.linearVelocity.x, 0f, rig.linearVelocity.z);
            rig.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
а а #endregion
}