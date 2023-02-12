using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImg;
    private GameManager _gameManager;
    public int bestScore;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.Log("Game Manager is NULL");
        }

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreText.text = "Best: " + bestScore;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void CheckForBestScore(int playerScore)
    {
        if(playerScore > bestScore)
        {
            bestScore = playerScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            _bestScoreText.text = "Best: " + bestScore;           
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
           GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResumePlay()
    {
        _gameManager.HidePauseMenuPanel();
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}