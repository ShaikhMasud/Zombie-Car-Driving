using UnityEngine;

public class ZombieMovement : MonoBehaviour {
    public Transform target; // The target (transform) to follow
    public float speed = 3f; // Movement speed of the zombie
    public float attackDistance = 1.5f; // Distance at which zombie will attack
    private Animator animator;
    private bool isAttacking = false; // To track if zombie is attacking

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("StartWalking"); // Start walking animation initially

        if (target == null)
        {
            Debug.LogWarning("Target not assigned in ZombieMovement script.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate the distance between zombie and the target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > attackDistance)
            {
                // If the zombie is farther away than the attack distance, move towards the target
                MoveTowardsTarget();

                // Stop attacking if it was attacking
                if (isAttacking)
                {
                    isAttacking = false;
                    animator.ResetTrigger("Attack"); // Stop attack animation
                    animator.SetTrigger("StartWalking"); // Return to walking animation
                }
            }
            else
            {
                // If the zombie is close enough to the target, trigger the attack animation
                if (!isAttacking)
                {
                    isAttacking = true;
                    animator.ResetTrigger("StartWalking"); // Stop walking animation
                    animator.SetTrigger("Attack"); // Start attack animation
                }
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (!isAttacking) // Make sure the zombie doesn't move during an attack
        {
            // Move zombie towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotate the zombie to face the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        }
    }
}