using UnityEngine;

public class GrassScatterUneven : MonoBehaviour
{
    [Header("Grass Settings")]
    public GameObject grassPrefab;       // Grass prefab
    public int numberOfGrass = 100;      // How many grass objects to scatter
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f); // Random scale
    public bool randomRotation = true;   // Random Y rotation
    public float maxRayHeight = 50f;     // Height above plane to cast from

    [Header("Plane Settings")]
    public Transform plane;              // The uneven plane to scatter on

    void Start()
    {
        if (grassPrefab == null || plane == null)
        {
            Debug.LogError("GrassPrefab or Plane is not assigned.");
            return;
        }

        ScatterGrass();
    }

    void ScatterGrass()
    {
        // Get plane bounds
        Vector3 planeSize = plane.localScale;
        Vector3 planePos = plane.position;

        float planeWidth = 10f * planeSize.x;  // Unity plane default is 10 units
        float planeLength = 10f * planeSize.z;

        for (int i = 0; i < numberOfGrass; i++)
        {
            // Random XZ position
            float xPos = Random.Range(-planeWidth / 2f, planeWidth / 2f);
            float zPos = Random.Range(-planeLength / 2f, planeLength / 2f);

            // Start raycast from above
            Vector3 rayOrigin = planePos + new Vector3(xPos, maxRayHeight, zPos);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxRayHeight * 2f))
            {
                // Instantiate grass at hit point
                GameObject grass = Instantiate(grassPrefab, hit.point, Quaternion.identity, this.transform);

                // Random rotation
                if (randomRotation)
                {
                    grass.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
                }

                // Random scale
                float scale = Random.Range(scaleRange.x, scaleRange.y);
                grass.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}