using UnityEngine;

// Handles everything about a single zombie — wandering and ragdoll on hit
// Implements IDamageable so HitDetector can trigger it
public class Zombie : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float minDuration = 2f;
    [SerializeField] private float maxDuration = 4f;

    private Animator _anim;
    private Rigidbody[] _bones;
    private Collider[] _boneColliders;

    private Vector3 _moveDir;
    private float _timer;
    private bool _isDead;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _bones = GetComponentsInChildren<Rigidbody>();
        _boneColliders = GetComponentsInChildren<Collider>();

        SetRagdoll(false);
    }

    private void OnEnable()
    {
        _isDead = false;
        _anim.enabled = true;
        SetRagdoll(false);
        PickDirection();
    }

    private void Update()
    {
        if (_isDead) return;

        transform.position += _moveDir * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_moveDir);

        _timer -= Time.deltaTime;
        if (_timer <= 0f) PickDirection();

        _anim.SetFloat("MoveSpeed", moveSpeed);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall")) PickDirection();
    }

    private void PickDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        _moveDir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        _timer = Random.Range(minDuration, maxDuration);
    }

    // IDamageable — called by HitDetector on car collision
    public void TakeDamage(Vector3 direction, float force)
    {
        if (_isDead) return;

        _isDead = true;
        SetRagdoll(true);
        ApplyForce(direction, force);

        RoundManager.Instance?.AddScore(1);
        ZombieManager.Instance.OnZombieDown();
    }

    private void SetRagdoll(bool on)
    {
        _anim.enabled = !on;

        foreach (var rb in _bones)
        {
            if (rb.transform == transform)
            {
                if (on)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
                continue;
            }
            rb.isKinematic = !on;
        }

        foreach (var col in _boneColliders)
        {
            if (col.transform == transform)
            {
                // Disable root capsule on death so car doesn't get stuck
                if (on) col.enabled = false;
                continue;
            }
            col.enabled = on;
        }
    }

    private void ApplyForce(Vector3 dir, float force)
    {
        foreach (var rb in _bones)
        {
            if (rb.transform == transform) continue;
            rb.AddForce(dir.normalized * force, ForceMode.Impulse);
        }
    }
}
