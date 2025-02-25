using UnityEngine;

public class DestroyFractures : MonoBehaviour
{
    public float speed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>().globalSpeed;

        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0, 0, -1);
        transform.Translate(direction * Time.deltaTime * speed);
    }
}
