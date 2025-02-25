using UnityEngine;
using UnityEngine.SceneManagement; 

public class Menu : MonoBehaviour
{

    private void Start()
    {

    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
