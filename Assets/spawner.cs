using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
    public GameObject zombiePrefab; // The prefab to spawn
    public float spawnRadius = 10f; // Radius around the spawner to spawn zombies
    public float minDistance = 2f; // Minimum distance between zombies
    public Transform target; // The target (transform) for zombies to follow

    private List<Vector3> spawnPositions = new List<Vector3>(); // List to track spawn positions

    void Start()
    {
        // Start spawning zombies every 5 seconds
        InvokeRepeating("SpawnZombie", 0f, 5f);
    }

    public void SpawnZombie()
    {
        Vector3 randomPosition;
        int attempts = 0;
        const int maxAttempts = 50; // Limit the number of attempts to avoid infinite loops

        do
        {
            // Generate a random position within a circular area around the spawner
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            float randomDistance = Random.Range(0f, spawnRadius);
            randomPosition = new Vector3(
                transform.position.x + randomDistance * Mathf.Cos(randomAngle),
                transform.position.y, // Keep Y fixed for ground-level spawning
                transform.position.z + randomDistance * Mathf.Sin(randomAngle)
            );

            attempts++;
        }
        while (!IsPositionValid(randomPosition) && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            // Instantiate the zombie prefab at the valid position
            GameObject zombie = Instantiate(zombiePrefab, randomPosition, Quaternion.identity);

            // Assign the target to the zombie's movement script
            if (target != null)
            {
                ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();
                if (zombieMovement != null)
                {
                    zombieMovement.target = target; // Set the target for the zombie
                }
            }

            spawnPositions.Add(randomPosition); // Track this zombie's position
        }
        else
        {
            Debug.LogWarning("Could not find a valid spawn position after " + maxAttempts + " attempts.");
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnPosition in spawnPositions)
        {
            if (Vector3.Distance(position, spawnPosition) < minDistance)
            {
                return false; // Position is too close to another zombie
            }
        }
        return true; // Position is valid
    }
}
