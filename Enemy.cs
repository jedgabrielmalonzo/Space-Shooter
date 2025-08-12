using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
   

    private AudioSource _audioSource;
    private Player _player;
    private Animator _anim;


    void Start()
    {
        transform.position = new Vector3(0, 7f, 0);

        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();


        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("The animator is Null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _audioSource.Play();

             _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;


            Destroy(this.gameObject, 1.5f);
            Destroy(GetComponent<Collider2D>());
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject); // Destroy the laser
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (player != null)
            {
                player.AddScore(1); // Award points for killing the enemy
            }
            _audioSource.Play();

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, 1.5f); // Destroy the enemy
            Destroy(GetComponent<Collider2D>());
        }
    }
}
