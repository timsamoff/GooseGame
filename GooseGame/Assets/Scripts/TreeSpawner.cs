using UnityEngine;
using System.Collections;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxPrefabsToInstantiate = 5;
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float leftBoundary = -1f;
    [SerializeField] private float rightBoundary = 1f;

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

        float screenWidth = Screen.width;
        float screenToWorldWidth = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, 0, 0)).x;

        for (int i = 0; i < numPrefabsToInstantiate; i++)
        {
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];

            float randomX;

            // Randomly choose whether to spawn on the left or right side
            if (Random.Range(0f, 1f) < 0.5f)
            {
                randomX = Random.Range(-screenToWorldWidth, leftBoundary); // Adjust this range based on your scene
            }
            else
            {
                randomX = Random.Range(rightBoundary, screenToWorldWidth); // Adjust this range based on your scene
            }

            float randomY = Random.Range(5f, 10f); // Adjust this range based on your scene

            instantiatedPrefabs[i] = Instantiate(randomPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
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

                    // Destroy prefabs when they leave the screen
                    if (prefabsToMove[i].transform.position.y < -10f)
                    {
                        Destroy(prefabsToMove[i]);
                    }
                }
            }

            yield return null;
        }
    }
}