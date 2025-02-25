using System;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    public ParticleSystem areaGlowVfx;
    private bool playerIsPizzaAvailable = false;
    private bool isVfxPlaying = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerIsPizzaAvailable = GameObject.Find("Player").GetComponent<PlayerController>().isPizzaAvailable;
    }

    // Update is called once per frame
    void Update()
    {
        this.HandleVfx();

        if(playerIsPizzaAvailable) gameObject.transform.Rotate(new Vector3(0f, 0.1f, 0f), Space.World);
    }

    private void HandleVfx()
    {
        playerIsPizzaAvailable = GameObject.Find("Player").GetComponent<PlayerController>().isPizzaAvailable;
        if (playerIsPizzaAvailable && !isVfxPlaying)
        {
            areaGlowVfx.Play();
            isVfxPlaying = true;
        }
        else
        {
            areaGlowVfx.Stop();
            isVfxPlaying = false;
        }
    }
}
