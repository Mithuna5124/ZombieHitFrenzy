using UnityEngine;

// Follows the player car smoothly from a fixed offset
public class CameraRig : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3   offset;
    [SerializeField] private float     followSpeed = 15f;

    private void LateUpdate()
    {
        if (followTarget == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            followTarget.position + offset,
            followSpeed * Time.deltaTime
        );
    }
}
