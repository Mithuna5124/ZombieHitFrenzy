using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the zombie pool — pre-creates all zombies, activates on demand
public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int activeCount = 10;
    [SerializeField] private int reserveCount = 5;
    [SerializeField] private float respawnDelay = 2.5f;
    [SerializeField] private float spawnRange = 18f;

    private List<GameObject> _pool = new();

    private void Awake() => Instance = this;

    private void Start()
    {
        for (int i = 0; i < activeCount + reserveCount; i++)
        {
            var z = Instantiate(zombiePrefab);
            z.SetActive(false);
            _pool.Add(z);
        }

        for (int i = 0; i < activeCount; i++)
            Activate(_pool[i]);
    }

    public void OnZombieDown() => StartCoroutine(RespawnRoutine());

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        var next = GetInactive();
        if (next != null) Activate(next);
    }

    private GameObject GetInactive()
    {
        foreach (var z in _pool)
            if (!z.activeInHierarchy) return z;
        return null;
    }

    private void Activate(GameObject zombie)
    {
        zombie.transform.position = RandomPoint();
        zombie.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        zombie.SetActive(true);
    }

    public Vector3 RandomPoint() => new(
        Random.Range(-spawnRange, spawnRange),
        0f,
        Random.Range(-spawnRange, spawnRange)
    );
}
