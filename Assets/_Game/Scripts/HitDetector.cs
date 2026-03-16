using UnityEngine;

// Sits on PlayerCar — detects collisions and calls TakeDamage on anything IDamageable
public class HitDetector : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 2f;
    [SerializeField] private float impactForce = 0.5f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) _rb = GetComponentInParent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb == null) return;
        if (_rb.linearVelocity.magnitude < speedThreshold) return;

        var target = collision.gameObject.GetComponent<IDamageable>()
                  ?? collision.gameObject.GetComponentInParent<IDamageable>();

        if (target == null) return;

        Vector3 dir = collision.contacts[0].normal * -1f;
        dir.y = 0.3f;

        target.TakeDamage(dir, impactForce * _rb.linearVelocity.magnitude);
    }
}
