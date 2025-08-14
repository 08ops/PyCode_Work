using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject prefab;     // The object you want to spawn
    public int amount = 5;        // How many to spawn
    public Vector3 areaSize = new Vector3(5, 0, 5); // Spawn area

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogError("⚠ No prefab assigned to TestSpawner!");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
