using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<float> trackXPositions;
    public int currentTrack;
    public int interpolationFramesCount = 100; // Number of frames to completely interpolate between the 2 positions
    private int elapsedFrames = 0;

    private float vfxTimeout = 3.0f;

    public int lives = 3;
    public int score = 0;
    public int pizzaStack = 0;
    public bool isPizzaAvailable = false;
    public bool needsRepair = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI pizzaText;
    public GameObject gameManager;

    public GameObject deliverPizzaVfx;
    public GameObject getPizzaVfx;
    public GameObject repairVfx;
    public GameObject asteroidCrashVfx;
    public GameObject asteroidCrashDeathVfx;
    public ParticleSystem smokeTrailVfx;

    public AudioSource switchLanesAudioSource;
    public AudioSource asteroidCrashAudioSource;
    private AudioSource getPizzaAudioSource;
    private AudioSource deliverPizzaAudioSource;
    private AudioSource powerupRepairAudioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeTracks();
        asteroidCrashAudioSource = transform.Find("AsteroidCrashFx").GetComponent<AudioSource>();
        switchLanesAudioSource = transform.Find("SwitchLanesFx").GetComponent<AudioSource>();
        getPizzaAudioSource = transform.Find("GetPizzaFx").GetComponent<AudioSource>();
        deliverPizzaAudioSource = transform.Find("DeliverPizzaFx").GetComponent<AudioSource>();
        powerupRepairAudioSource = transform.Find("PowerupRepairFx").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GetComponent<GameManager>().isGameOver) {
            MovePlayer(); 
        }
    }

    //Detects horizontal input and moves player between tracks
    private void MovePlayer()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        if ((Input.GetKeyDown("right") || Input.GetKeyDown("d")) && currentTrack <= 2)
        {
            switchLanesAudioSource.Play();
            currentTrack += 1;
        }
        else if ((Input.GetKeyDown("left") || Input.GetKeyDown("a")) && currentTrack >= 1)
        {
            switchLanesAudioSource.Play();
            currentTrack -= 1;
        }

        
        Vector3 destination = new Vector3(trackXPositions[currentTrack], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destination, interpolationRatio);

        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)
    }

    private void InitializeTracks()
    {
        trackXPositions = new List<float> { -4.0f, -1.5f, 1.0f, 3.5f };
        currentTrack = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            OnAsteroidCrash(other);
        }
        else if (other.CompareTag("Customer") && pizzaStack > 0)
        {
            OnDeliverPizza(other);
        }
        else if (other.CompareTag("Shop"))
        {
            OnGetPizza(other);
        }
        else if (other.CompareTag("PowerupRepair"))
        {
            OnRepairPowerup(other);
        }
    }

    private void OnAsteroidCrash(Collider asteroid)
    {
        asteroidCrashAudioSource.Play();

        lives -= 1;
        needsRepair = lives < 3;

        if(lives <= 0)
        {
            GameObject instance = Instantiate(asteroidCrashDeathVfx, asteroid.transform.position, deliverPizzaVfx.transform.rotation);
            Destroy(instance, vfxTimeout);
            smokeTrailVfx.Stop();
        }
        else
        {
            GameObject instance = Instantiate(asteroidCrashVfx, asteroid.transform.position, deliverPizzaVfx.transform.rotation);
            Destroy(instance, vfxTimeout);
        }

        Destroy(asteroid.gameObject);
        asteroid.gameObject.GetComponentInChildren<Fracture>().FractureObject();
        livesText.text = "Lives: " + lives;
    }

    private void OnGetPizza(Collider pizza)
    {
        getPizzaAudioSource.Play();

        pizzaStack += 1;
        isPizzaAvailable = true;

        GameObject instance = Instantiate(getPizzaVfx, pizza.transform.position, getPizzaVfx.transform.rotation);
        Destroy(pizza.gameObject);
        Destroy(instance, vfxTimeout);

        pizzaText.text = pizzaStack + " Pizzas";
    }

    private void OnDeliverPizza(Collider customer)
    {
        deliverPizzaAudioSource.Play();

        pizzaStack -= 1;
        score += 10;
        isPizzaAvailable = pizzaStack > 0;

        GameObject instance = Instantiate(deliverPizzaVfx, customer.transform.position, deliverPizzaVfx.transform.rotation);
        Destroy(customer.gameObject);
        Destroy(instance, vfxTimeout);

        pizzaText.text = pizzaStack + " Pizzas";
        scoreText.text = "Score: " + score;
    }

    private void OnRepairPowerup(Collider powerup)
    {
        if (lives < 3)
        {
            powerupRepairAudioSource.Play();
            lives += 1;
            needsRepair = lives < 3;

            GameObject instance = Instantiate(repairVfx, powerup.transform.position, repairVfx.transform.rotation);
            Destroy(powerup.gameObject);
            Destroy(instance, vfxTimeout); livesText.text = "Lives: " + lives;
        }
    }

}
