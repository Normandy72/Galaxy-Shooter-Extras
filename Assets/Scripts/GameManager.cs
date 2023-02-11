using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    public bool isCoopMode = false;

    void Update()
    {
        if(_isGameOver == true)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                int index = SceneManager.GetActiveScene().buildIndex;   // current game scene
                SceneManager.LoadScene(index);
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);  // main menu
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void GameOver()
    {
        _isGameOver = true;
    }
}
