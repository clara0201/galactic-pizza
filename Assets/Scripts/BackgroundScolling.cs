using UnityEngine;

public class BackgroundScolling : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    public float speed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>().globalSpeed;

        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
        Debug.Log(repeatWidth);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        transform.Translate(direction * Time.deltaTime * speed); 
        
        if (transform.position.z < startPos.z - repeatWidth) {
            transform.position = startPos;
        }
    }
}
