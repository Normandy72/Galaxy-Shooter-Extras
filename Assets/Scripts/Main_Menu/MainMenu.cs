using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerManager()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadCoOpGame()
    {
        SceneManager.LoadScene(2);
    }
}