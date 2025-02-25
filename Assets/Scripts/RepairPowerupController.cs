using System;
using UnityEngine;

public class RepairPowerupController : MonoBehaviour
{
    public ParticleSystem areaGlowVfx;
    private bool playerNeedsRepair = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNeedsRepair = GameObject.Find("Player").GetComponent<PlayerController>().needsRepair;
    }

    // Update is called once per frame
    void Update()
    {
        this.HandleVfx();
       
        gameObject.transform.Rotate(new Vector3(0f, 0.1f, 0f), Space.World);
    }

    private void HandleVfx()
    {
        playerNeedsRepair = GameObject.Find("Player").GetComponent<PlayerController>().needsRepair;
        if (playerNeedsRepair && !areaGlowVfx.isPlaying)
            areaGlowVfx.Play();
        else if(!playerNeedsRepair && areaGlowVfx.isPlaying)
            areaGlowVfx.Stop();
    }
}
