using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkAnimator))]
public class PlayerAnimation : NetworkBehaviour
{
    private Animator animator;
    private NetworkAnimator networkAnimator;
    private static readonly int HashIsMoving = Animator.StringToHash("IsMoving");
    private static readonly int HashTaunt = Animator.StringToHash("Taunt");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        float move = 0f;
        float turn = 0f;

        if (keyboard.wKey.isPressed)
        {
            move += 1f;
        }
        if (keyboard.sKey.isPressed)
        {
            move -= 1f;
        }
        if (keyboard.dKey.isPressed)
        {
            turn += 1f;
        }
        if (keyboard.aKey.isPressed)
        {
            turn -= 1f;
        }

        animator.SetBool(HashIsMoving, move != 0f);

        if (keyboard.tKey.wasPressedThisFrame)
        {
            networkAnimator.SetTrigger(HashTaunt);
        }
    }
}
