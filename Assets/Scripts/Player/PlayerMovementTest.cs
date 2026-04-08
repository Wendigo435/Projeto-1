using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerMovementTest : NetworkBehaviour
{
    [Header("Movement")]
    public float MoveSpd;
    public float WalkSpd = 5f;
    public float RunSpd = 10f;
    public bool isRun;
    public Vector2 MoveInput;

    [Header("Jump")]
    public float JumpForce = 12f;

    [Header("GroundCheck")]
    public Transform GroundCheck;
    public float GroundDis = 0.2f;
    public LayerMask GroundLayer;
    public bool isGrounded;

    [Header("Components")]
    Rigidbody rig;


    void Start()
    {
        rig = GetComponent<Rigidbody>();
        if (!isLocalPlayer) { enabled = false; return; }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDis, GroundLayer);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isRun)
        {
            MoveSpd = RunSpd;
        }
        else
        {
            MoveSpd = WalkSpd;
        }
    }
}
