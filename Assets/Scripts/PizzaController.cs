using UnityEngine;

public class PizzaController : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0f, 0.1f, 0f), Space.World);
    }
}
