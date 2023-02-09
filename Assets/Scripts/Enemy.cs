using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;

    void Start()
    {
        _player = FindObjectOfType<Player>();

        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _animator = GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("The Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the enemy is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Player player = other.transform.GetComponent<Player>();

        if(other.tag == "Player")
        {
            if(_player != null)
            {
                _player.Damage();
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);            
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore(10);
            }         

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);            
        }
    }
}