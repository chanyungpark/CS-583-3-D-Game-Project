using UnityEngine;

public class GrassScatterUneven : MonoBehaviour
{
    [Header("Grass Settings")]
    public GameObject grassPrefab;
    public int numberOfClusters = 100;          // Number of clusters (not blades)
    public int grassPerCluster = 7;             // Fixed cluster size
    public float clusterRadius = 0.4f;          // How spread out each cluster is
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public bool randomRotation = true;
    public float maxRayHeight = 50f;

    [Header("Plane Settings")]
    public Transform plane;

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
        Vector3 planeSize = plane.localScale;
        Vector3 planePos = plane.position;

        float planeWidth = 10f * planeSize.x;
        float planeLength = 10f * planeSize.z;

        for (int i = 0; i < numberOfClusters; i++)
        {
            // Choose a cluster center
            float xPos = Random.Range(-planeWidth / 2f, planeWidth / 2f);
            float zPos = Random.Range(-planeLength / 2f, planeLength / 2f);

            Vector3 clusterRayOrigin = planePos + new Vector3(xPos, maxRayHeight, zPos);

            if (!Physics.Raycast(clusterRayOrigin, Vector3.down, out RaycastHit clusterHit, maxRayHeight * 2f))
                continue;

            // Spawn grass inside the cluster
            for (int j = 0; j < grassPerCluster; j++)
            {
                Vector2 offset2D = Random.insideUnitCircle * clusterRadius;
                Vector3 offset = new Vector3(offset2D.x, 0f, offset2D.y);

                Vector3 rayOrigin = clusterHit.point + offset + Vector3.up * maxRayHeight;

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxRayHeight * 2f))
                {
                    GameObject grass = Instantiate(grassPrefab, hit.point, Quaternion.identity, this.transform);

                    if (randomRotation)
                        grass.transform.Rotate(0f, Random.Range(0f, 360f), 0f);

                    float scale = Random.Range(scaleRange.x, scaleRange.y);
                    grass.transform.localScale = Vector3.one * scale;
                }
            }
        }
    }
}
