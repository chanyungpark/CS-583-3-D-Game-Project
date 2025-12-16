
using UnityEngine;

public class FireMechanics : MonoBehaviour
{
    [Tooltip("Growth")]
    public float minScale = .2f;
    public float maxScale = 1.5f;
    public float growthTime = 30f;

    [Tooltip("Spread")]
    public GameObject firePrefab;
    public float spreadInterval = 3f;
    public float spreadRadius = 4f;
    public int maxChildren = 5;

    [Tooltip("Placement")]
    public LayerMask groundMask;

    private float growthTimer = 0f;
    private float spreadTimer = 0f;
    private int spawnedChildren = 0;
    private bool IsMature => growthTimer >= growthTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * minScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGrowth();
        HandleSpread();
    }

    public void HandleGrowth()
    {
        if(growthTimer < growthTime)
        {
            growthTimer += Time.deltaTime;
            float t = Mathf.Clamp01(growthTimer / growthTime);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = Vector3.one * scale;
        }
    }

    public void HandleSpread()
    {
        if (!IsMature || spawnedChildren >= maxChildren || firePrefab == null)
            return;

        spreadTimer += Time.deltaTime;
        if (spreadTimer >= spreadInterval)
        {
            spreadTimer = 0f;
            
            TrySpawnNewFire();
        }
    }

    public void TrySpawnNewFire()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spreadRadius;
        Vector3 spawnPos = transform.position + new UnityEngine.Vector3(randomCircle.x, 5f, randomCircle.y);

        if(Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 10f, groundMask))
        {
            Instantiate(firePrefab, hit.point, Quaternion.identity);
            spawnedChildren++;
        }
    }
}
