using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;

    [Header("Common")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score = 0;

    [Header("Laser")]
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.5f;

    [Header("Effects")]
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngine, _rightEngine;
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    private float _canFire = -1f;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    
    
    void Start()
    {
        // _spawnManager = FindObjectOfType<SpawnManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager on the player is NULL");
        }

        // position
        if(_gameManager.isCoopMode == false)
        {
            transform.position  = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        if(isPlayerOne == true)
        {
            CalculateMovement();

            if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        
        if(isPlayerTwo == true)
        {
            Player2Movement();

            if(Input.GetKeyDown(KeyCode.RightShift) && Time.time > _canFire)
            {
                FireLaser();
            }            
        }
    }

    void CalculateMovement()
    {
        // --- user input ---
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // Vector3.right == new Vector3(1, 0, 0);
        // Vector3.up == new Vector3(0, 1, 0);
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        transform.Translate(direction * _speed * Time.deltaTime);        

        // --- bounds ---
        // if(transform.position.y >= 0)
        // {
        //     transform.position = new Vector3(transform.position.x, 0, 0);
        // }
        // else if(transform.position.y <= -3.8f)
        // {
        //     transform.position = new Vector3(transform.position.x, -3.8f, 0);
        // }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if(transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void Player2Movement()
    {
        if(Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

         if(Input.GetKey(KeyCode.Keypad2))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if(transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _uiManager.CheckForBestScore(_score);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}