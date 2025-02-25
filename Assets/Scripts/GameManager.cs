using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public GameObject player;
    public GameObject gameOverScreen;
    public GameObject scoresScreen;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI currentTimeText;


    public AudioSource gameOverAudioSource;
    private float totalTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameOverAudioSource = transform.Find("GameOverSx").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MenuScene");
        }

        if (player.GetComponent<PlayerController>().lives == 0 && !isGameOver)
        {
            GameOver();
        }

        if(!isGameOver)
            HandleTimeDisplay();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        gameOverAudioSource.Play();
        finalScoreText.text = "Final score: " + player.GetComponent<PlayerController>().score;
        finalTimeText.text = "Time: " + totalTime.ToString("0.00") + " s";
        isGameOver = true;
        gameOverScreen.SetActive(true);
        scoresScreen.SetActive(false);
    }

    private void HandleTimeDisplay()
    {
        totalTime += Time.deltaTime;
        currentTimeText.text = totalTime.ToString("0.00") + " s";
    }
}
