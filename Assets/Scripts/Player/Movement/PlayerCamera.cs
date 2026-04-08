using UnityEngine;
using Mirror;
using Mirror.Examples.Benchmark;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Componentes Externos")]
    public Transform CameraTarget;
    public Transform FPSTarget;
    public Camera Cam;

    [Header("TPS")]
    public float TPSdis = 4f;
    public float TPSalt = 1.5f;
    public float TPScollission = 0.2f;
    public LayerMask CollissionLayers;

    [Header("FPS")]
    public float FPSminY = -80f;
    public float FPSmaxY = 80f;


    [Header("Geral")]
    public float Sense = 3f;
    public float SmoothSpd = 15f;

    #region Privado
    private float rotX;
    private float rotY;
    private bool isFPS = false;
    private PlayerMovement PlayerMovement;
    #endregion

    void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer) return;

        Cam = Camera.main;
        rotY = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        if (InventoryUI.isOpen) return;

        HandleInput();
        HandleCamera();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (InventoryUI.isOpen) return;

        RotatePlayer(); // Agora roda junto com a física
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFPS = !isFPS;
        }

        rotY += Input.GetAxis("Mouse X") * Sense;
        rotX -= Input.GetAxis("Mouse Y") * Sense;

        if (isFPS)
        {
            rotX = Mathf.Clamp(rotX, FPSminY, FPSmaxY);
        }
        else
        {
            rotX = Mathf.Clamp(rotX, -40f, 70f);
        }
    }

    void HandleCamera()
    {
        if (isFPS)
        {
            HandleFPS();
        }
        else
        {
            HandleTPS();
        }
    }

    void HandleFPS()
    {
        Cam.transform.position = FPSTarget.position;

        Cam.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void HandleTPS()
    {
        Quaternion camRotation = Quaternion.Euler(rotX, rotY, 0);
        Vector3 camDirection = camRotation * Vector3.back;

        Vector3 idealPosition = CameraTarget.position + camDirection * TPSdis + Vector3.up * TPSalt;

        // Colisão — verifica se tem obstáculo entre o player e a câmera
        Vector3 finalPosition = idealPosition;
        if (Physics.SphereCast(
            CameraTarget.position,          // Origem (peito do player)
            TPScollission,             // Raio
            camDirection,                   // Direção
            out RaycastHit hit,             // O que bateu
            TPSdis,                    // Distância máxima
            CollissionLayers))            // Layers que colidem
        {
            // Aproxima a câmera do player no ponto de colisão
            finalPosition = CameraTarget.position + camDirection * (hit.distance - TPScollission) + Vector3.up * TPSalt;
        }
        Cam.transform.position = Vector3.Lerp(Cam.transform.position, finalPosition, Time.deltaTime * SmoothSpd);
        Cam.transform.LookAt(CameraTarget.position + Vector3.up * 0.5f); // Olha pro peito do player
    }

    void RotatePlayer()
    {
        if (isFPS)
        {
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
        else
        {
            if (PlayerMovement.MoveX == 0 && PlayerMovement.MoveY == 0) return;

            // Rotação alvo baseada na câmera
            float targetRotY = Mathf.MoveTowardsAngle(
                transform.eulerAngles.y,
                rotY,
                SmoothSpd * 10f * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(0, targetRotY, 0);
        }
    }
}

