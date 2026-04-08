using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [Header("Gravidade Extra")]
    Rigidbody rig;
    public float gravityMultiplier = 2.5f;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyExtraGravity();
    }

    void ApplyExtraGravity()
    {
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        rig.AddForce(extraGravityForce, ForceMode.Acceleration);
    }
}
