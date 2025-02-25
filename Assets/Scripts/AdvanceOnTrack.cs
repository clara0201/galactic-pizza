using UnityEngine;

public class AdvanceOnTrack : MonoBehaviour
{
    public float speed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>().globalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0f, 0f, -1f);
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (transform.position.z < -10f) { Destroy(gameObject); }
    }
}
