using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicKicker : NetworkBehaviour
{
    public float m_KickRadius = 3f;
    public float m_KickForce = 3f;
    public float m_Upward = 0.4f;

    private void Update()
    {
        if (!IsOwner)
            return;

        var keyboard = Keyboard.current;
        if (keyboard.tKey.wasPressedThisFrame)
            RequestKickRpc();
    }

    [Rpc(SendTo.Server)]
    private void RequestKickRpc()
    {
        var colliders = Physics.OverlapSphere(transform.position, m_KickRadius);
        foreach(var collider in colliders)
        {
            var body = collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                continue;

            var dir = transform.forward;
            dir.y += m_Upward;
            dir.Normalize();
            body.AddForce(dir * m_KickForce, ForceMode.Impulse);    
        }
    }
}
