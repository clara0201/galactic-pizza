using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject meteorPrefab;
    public GameObject shopPrefab;
    public GameObject customerPrefab;
    public GameObject powerupPrefab;
    public GameObject gameManager;

    public List<float> trackXPositions;

    public float startTimeMeteorSpawn = 1.0f;
    public float meteorSpawnInterval = 3.0f;

    public float startTimeShopSpawn = 5.0f;
    public float shopSpawnInterval = 5.0f;

    public float startTimeCustomerSpawn = 7.0f;
    public float customerSpawnInterval = 8.0f;

    private float timeInterval = 1f; 
    private float minInterval = 0.1f; 
    private float intervalDecrease = 0.1f; // Decrease interval by 0.1 seconds every 10 seconds
    private float timer = 0f; // To keep track of elapsed time for interval decrease

    public float globalSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTracks();

        //TODO using global variables makes it weird for some reason
        InvokeRepeating("SpawnShop", 5.0f, 5.5f);
        InvokeRepeating("SpawnCustomer", 6.0f, 4.5f);
        InvokeRepeating("SpawnPowerup", 15.0f, 15f);

        StartCoroutine("CallSpawnMeteorIWithIncreasingInterval");
    }

    // Update is called once per frame
    void Update()
    {

        globalSpeed = globalSpeed + 0.1f * Time.deltaTime;

        if (gameManager.GetComponent<GameManager>().isGameOver)
        {
            CancelInvoke();
            StopAllCoroutines();
        }
    }

    private void InitializeTracks()
    {
        trackXPositions = new List<float> { -4.0f, -1.5f, 1.0f, 3.5f };
    }

    IEnumerator CallSpawnMeteorIWithIncreasingInterval()
    {
        while (true)
        {
            // Call the method
            SpawnMeteor();

            // Wait for the current interval
            yield return new WaitForSeconds(timeInterval);

            // Update the timer
            timer += timeInterval;

            // Every 10 seconds, decrease the interval by 'intervalDecrease'
            if (timer >= 10f)
            {
                timeInterval -= intervalDecrease;
                if (timeInterval < minInterval)
                {
                    timeInterval = minInterval;
                }
                timer = 0f; // Reset the timer after reducing the interval
            }
        }
    }

    // Spawns between 1 and 3 meteors (randomly) at a time if there's enough free tracks
    private void SpawnMeteor()
    {
        for(int i = 1; i < Random.Range(2, 4); i++)
        {
            Vector3 position = GenerateSpawnPosition(trackXPositions);
            if(position != Vector3.zero)
                Instantiate(meteorPrefab, position, meteorPrefab.transform.rotation);
        }
    }

    // Spawns 1 shop if there's a free track
    private void SpawnShop()
    {
        //Debug.Log("Spawn shop");
        Vector3 position = GenerateSpawnPosition(trackXPositions);
        if (position != Vector3.zero)
            Instantiate(shopPrefab, position, shopPrefab.transform.rotation);
    }

    // Spawns 1 shop if there's a free track
    private void SpawnCustomer()
    {
        //Debug.Log("Spawn customer");
        Vector3 position = GenerateSpawnPosition(trackXPositions);
        if (position != Vector3.zero)
            Instantiate(customerPrefab, position, customerPrefab.transform.rotation);
    }
    
    private void SpawnPowerup()
    {
        Vector3 position = GenerateSpawnPosition(trackXPositions);
        if (position != Vector3.zero)
            Instantiate(powerupPrefab, position, powerupPrefab.transform.rotation);
    }

    // Returns a Vector3 with a random free position, or Vector3.zero if no tracks are free at the current time
    private Vector3 GenerateSpawnPosition(List<float> trackXValues)
    {
        int spawnTrackIndex = Random.Range(0, trackXValues.Count);
        Vector3 position = new Vector3(trackXPositions[spawnTrackIndex], 0.5f, 10.0f);

        if (IsTrackFree(position))
        {
            //Debug.Log("found free position at track " + spawnTrackIndex);
            return position;
        }
        if (trackXValues.Count == 1)
        {
            //Debug.Log("no free tracks");
            return Vector3.zero;
        }
        else
        {
            //Debug.Log("tried to place at track " + spawnTrackIndex + " but was occupied");
            
            List<float> trackXValuesUpdated = new List<float>(trackXValues);
            trackXValuesUpdated.RemoveAt(spawnTrackIndex);

            return GenerateSpawnPosition(trackXValuesUpdated);
        }
    }

    // Checks if a specific track is free at the current time
    bool IsTrackFree(Vector3 position)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.04f);

        return intersecting.Length == 0;
    }
}
