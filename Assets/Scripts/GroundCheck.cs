using UnityEngine;


public class GroundCheck : MonoBehaviour
{
    private PlayerController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }


    private void DrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, controller.groundCheckRadius);
    }
}