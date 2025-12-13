using UnityEngine;

public class FloatAndSpin : MonoBehaviour
{
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 1f;
    public float rotateSpeed = 45f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Float
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Spin
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }
}