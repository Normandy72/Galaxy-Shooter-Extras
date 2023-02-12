using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    public bool isCoopMode = false;
    [SerializeField] private GameObject _pauseMenuPanel;
    private Animator _pauseAnimator;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    

    void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

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

        if(Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
    }
    
    public void GameOver()
    {
        _isGameOver = true;
    }

    public void HidePauseMenuPanel()
    {
        _pauseMenuPanel.SetActive(false);
    }
}
