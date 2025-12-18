using UnityEngine;

public class HikerSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject hikerPrefab;
    public int hikersToSpawn = 6;
    public int spawnedhikers = 0;

    [Header("Spawn Zones")]
    public BoxCollider[] spawnZones;


    [Header("Placement")]
    public LayerMask groundMask;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < hikersToSpawn; i++)
        {
            SpawnHiker();
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.hickersSpawned = spawnedhikers;
            GameManager.Instance.totalHikersInMap = spawnedhikers;
        }

    }

    void SpawnHiker()
    {
        if(spawnZones == null || spawnZones.Length == 0 || hikerPrefab == null)
        {
            Debug.LogWarning("HikerSpawner not set up correctly (missing zones or prefab).");
            return;
        }

        BoxCollider zone = spawnZones[Random.Range(0, spawnZones.Length)];
        Bounds b = zone.bounds;

        for(int attempts = 0; attempts < 10; attempts++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(b.min.x, b.max.x),
                b.max.y + 10f,
                Random.Range(b.min.z, b.max.z)
            );

            if(Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100f, groundMask))
            {
                Instantiate(hikerPrefab, hit.point, Quaternion.identity);
                spawnedhikers++;
                return;
            }
        }

        Debug.LogWarning("Failed to find ground for hiker in zone " + zone.name);
        
    }
}
