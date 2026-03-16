using UnityEngine;

public class ZombieWander : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float minWanderTime = 0.5f;
    [SerializeField] private float maxWanderTime = 4f;

    private Vector3 wanderDirection;
    private Transform carTransform;
    private float wanderTimer;
    private bool isDead = false;
    private Animator animator;

    private void Awake()
    {
        carTransform = GameObject.FindGameObjectWithTag("Player").transform;        
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Every time this zombie is activated from pool, reset it
        isDead = false;
        animator.enabled = true;
        PickNewDirection();
    }

    private void Update()
    {
        if (isDead) return;

        // Walk in current direction
        transform.position += wanderDirection * moveSpeed * Time.deltaTime;

        // Face the direction of movement
        transform.rotation = Quaternion.LookRotation(wanderDirection);

        // Count down timer
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            PickNewDirection();
            wanderTimer = Random.Range(minWanderTime, maxWanderTime);
        }

        // Update walk animation
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    private void PickNewDirection()
    {
        // Random angle on the horizontal plane
        float angle = Random.Range(0f, 360f);
      
        Vector3 targetDirection = new Vector3(
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(angle * Mathf.Deg2Rad)
        );
        float radius = Random.Range(9f, 15f);
        
        wanderDirection = (carTransform.position + targetDirection*radius - transform.position).normalized;
        //Quaternion rotateAxis = Quaternion.AngleAxis(angle, Vector3.up); ;
        //wanderDirection = rotateAxis * wanderDirection;

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Bounce off arena walls - pick new direction
        if (collision.gameObject.CompareTag("Wall"))
            PickNewDirection();
    }

    public void OnDeath()
    {
        isDead = true;
        animator.enabled = false;
    }
}