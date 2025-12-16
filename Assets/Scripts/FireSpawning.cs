using System.IO.Compression;
using UnityEngine;


public class FireSpawning : MonoBehaviour
{

    public GameObject firePrefab;
    public int initialFires = 10;
    public LayerMask groundMask;
    public Collider groundCollider;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialFires; i++)
        {
            SpawnRandomFire();
        }
    }


    void SpawnRandomFire()
    {
        Bounds b = groundCollider.bounds;

        Vector3 randomPos = new Vector3(
            Random.Range(b.min.x, b.max.x),
            b.max.y + 20f,
            Random.Range(b.min.z, b.max.z)
        );

        if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100f, groundMask))
        {
            Instantiate(firePrefab, hit.point, Quaternion.identity);
        }
    }
}
