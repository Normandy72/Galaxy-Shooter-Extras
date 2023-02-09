using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    // ID for powerups
    // 0 = triple shot
    // 1 = speed
    // 2 = shields
    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _clip;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(player != null)
            {
                switch(_powerupID)
                {
                    case 0: 
                        player.TripleShotActive();
                        break;
                    case 1: 
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }            
            
            Destroy(this.gameObject);
        }
    }
}