// ObjectSpawner.cs
using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxPrefabsToInstantiate = 5;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float leftBoundary = -1f;
    [SerializeField] private float rightBoundary = 1f;
    [SerializeField] private int maxMissesBeforeGameOver = 3;

    private int missesCount = 0; // Counter for misses

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnPrefabs();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnPrefabs()
    {
        int numPrefabsToInstantiate = Random.Range(1, maxPrefabsToInstantiate + 1);

        GameObject[] instantiatedPrefabs = new GameObject[numPrefabsToInstantiate];

        for (int i = 0; i < numPrefabsToInstantiate; i++)
        {
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
            float randomX = Random.Range(leftBoundary, rightBoundary);
            float randomY = Random.Range(5f, 10f);

            instantiatedPrefabs[i] = Instantiate(randomPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
            instantiatedPrefabs[i].name = "Object";

            // Attach the MissTracker script if not already attached
            MissTracker missTracker = instantiatedPrefabs[i].GetComponent<MissTracker>();
            if (missTracker == null)
            {
                missTracker = instantiatedPrefabs[i].AddComponent<MissTracker>();
                missTracker.objectSpawner = this;
            }
        }

        StartCoroutine(MovePrefabs(instantiatedPrefabs));
    }

    private IEnumerator MovePrefabs(GameObject[] prefabsToMove)
    {
        while (true)
        {
            for (int i = 0; i < prefabsToMove.Length; i++)
            {
                if (prefabsToMove[i] != null)
                {
                    prefabsToMove[i].transform.Translate(Vector3.down * speed * Time.deltaTime);

                    // Check if the prefab leaves the screen
                    if (prefabsToMove[i].transform.position.y < -10f)
                    {
                        // Increment misses only if the prefab is not tagged as "poop"
                        if (!prefabsToMove[i].CompareTag("poop"))
                        {
                            IncrementMisses();
                        }

                        // Destroy the prefab
                        Destroy(prefabsToMove[i]);
                    }
                }
            }

            yield return null;
        }
    }

    private void Update()
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (missesCount >= maxMissesBeforeGameOver)
        {
            Debug.Log("Game Over");
        }
    }

    public void IncrementMisses()
    {
        missesCount++;
        Debug.Log("Misses: " + missesCount);
    }
}