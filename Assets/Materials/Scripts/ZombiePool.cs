using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    public static ZombiePool Instance { get; private set; }

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int activeCount = 10;
    [SerializeField] private float arenaSize = 18f;
    [SerializeField] private float respawnDelay = 2.5f;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        int totalCount = 30;

        for (int i = 0; i < totalCount; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            pool.Add(zombie);
        }

        // Activate the first 10
        for (int i = 0; i < activeCount; i++)
        {
            ActivateZombie(pool[i]);
        }
    }

    // Called by ZombieRagdoll when a zombie gets hit
    public void OnZombieDied()
    {
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Find a zombie that is inactive in the pool
        GameObject zombie = GetInactiveZombie();

        if (zombie != null)
            ActivateZombie(zombie);
    }

    private GameObject GetInactiveZombie()
    {
        foreach (GameObject zombie in pool)
        {
            if (!zombie.activeInHierarchy)
                return zombie;
        }

        // Pool is exhausted - all zombies have been hit
        // This only happens very late in the round, it's fine
        return null;
    }

    private void ActivateZombie(GameObject zombie)
    {
        zombie.transform.position = GetRandomPosition();
        zombie.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        zombie.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-arenaSize, arenaSize),
            0f,
            Random.Range(-arenaSize, arenaSize)
        );
    }
}