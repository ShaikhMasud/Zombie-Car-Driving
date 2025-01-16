using UnityEngine;

public class CarHa : MonoBehaviour {
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform gameModel;

    [Header("Car Settings")]
    float maxSteerVelocity = 1;
    float maxSpeedLimit = 30;
    float accceleration = 3;
    float brakeAcceleration = 15;
    float steerAcceleration = 2;

    [Header("Game Settings")]
    public int maxCollisions = 30; // Max collisions before game over
    private int currentCollisions = 0;
    public bool hasWon = false;

    Vector2 input = Vector2.zero;

    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);

        // Check if game is over
        if (currentCollisions >= maxCollisions && !hasWon)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        if (input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.drag = 0.2f;
        }
        if (input.y < 0)
        {
            Brake();
        }
        Steer();
        if (rb.velocity.z <= 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Accelerate()
    {
        rb.drag = 0;
        if (rb.velocity.z > maxSpeedLimit)
        {
            return;
        }
        rb.AddForce(rb.transform.forward * accceleration * input.y);
    }

    void Brake()
    {
        if (rb.velocity.z <= 0)
        {
            return;
        }
        rb.AddForce(rb.transform.forward * brakeAcceleration * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.y) > 0)
        {
            float steerSpeedLimit = rb.velocity.z / 5.0f;
            steerSpeedLimit = Mathf.Clamp01(steerSpeedLimit);
            rb.AddForce(rb.transform.right * steerAcceleration * input.x * steerSpeedLimit);
            float normalizedX = rb.velocity.x * maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);
            rb.velocity = new Vector3(maxSteerVelocity * normalizedX, 0, rb.velocity.z);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasWon)
        {
            currentCollisions++;
            Debug.Log($"Collisions: {currentCollisions}/{maxCollisions}");

            if (currentCollisions >= maxCollisions)
            {
                GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            hasWon = true;
            WinGame();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Add your game over logic here (e.g., reload the scene)
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        // Add your win logic here (e.g., load the next scene or display a win message)
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    }
}
