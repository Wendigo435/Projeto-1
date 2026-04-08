using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
    [Header("Vida")]
    public float VidaMax = 100f;

    [SyncVar(hook = nameof(OnChangeHealth))]
    public float VidaAt;

    [Header("Knockback")]
    public float KBDistance = 5f;
    public float KBDuraction = 0.2f;
    public float KBX = 20f;
    public float KBY = 10f;
    public bool isKB;

    [Header("Barra Flutuante")]
    public Image BarraFlut;
    private Canvas canvasFlut;

    [Header("Barra na HUD")]
    public Image barraHUD;

    private Rigidbody rig;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();

        if (BarraFlut != null)
        {
            canvasFlut = BarraFlut.GetComponentInParent<Canvas>();
        }
    }

    public override void OnStartServer()
    {
        VidaAt = VidaMax;
    }

    public override void OnStartLocalPlayer()
    {
        GameObject hudObj = GameObject.Find("HealthBar");

        if (hudObj != null)
        {
            barraHUD = hudObj.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("HealthBar não encontrada na cena.");
        }

        if (canvasFlut != null)
        {
            canvasFlut.enabled = false;
        }

        UpdateUI(VidaAt);
    }

    void OnChangeHealth(float oldVida, float newVida)
    {
        UpdateUI(newVida);
    }

    void UpdateUI(float valor)
    {
        float preenchimento = valor / VidaMax;

        if (BarraFlut != null)
        {
            BarraFlut.fillAmount = preenchimento;
        }

        if (isLocalPlayer && barraHUD != null)
        {
            barraHUD.fillAmount = preenchimento;
        }
    }

    void LateUpdate()
    {
        if (canvasFlut == null) return;
        if (!canvasFlut.enabled) return;
        if (Camera.main == null) return;

        canvasFlut.transform.forward = Camera.main.transform.forward;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdTakeDamage(10f);
        }
    }

    [Command]
    void CmdTakeDamage(float amount)
    {
        VidaAt -= amount;

        if (VidaAt < 0)
            VidaAt = 0;

        if (VidaAt == 0)
        {
            Debug.Log("Player morreu no servidor");
        }
    }
    [Server]
    public void TakeDamage(float amount, Vector3 damageOrigin)
    {
        if (VidaAt <= 0) return;

        VidaAt -= amount;
        if (VidaAt < 0) VidaAt = 0;

        RpcKnockback(damageOrigin);

        if (VidaAt == 0)
        {
            Debug.Log($"{gameObject.name} morreu!");
        }
    }

    [ClientRpc]
    void RpcKnockback(Vector3 damageOrigin)
    {
        if (!isLocalPlayer) return;

        Vector3 direction = (transform.position - damageOrigin).normalized;
        Vector3 force = new Vector3(direction.x * KBX, KBY, direction.z * KBX);
        rig.AddForce(force, ForceMode.Impulse);
    }
}