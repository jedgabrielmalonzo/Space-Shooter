using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Declare _speed as a private field, with a default value
    [SerializeField] private float _speed = 3.0f; // You can adjust this value in the Unity Inspector
    [SerializeField] private int powerupID;
    [SerializeField] private AudioClip _clip;
    void Update()
    {
        // Move the power-up down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Destroy if it goes off-screen
        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object collided with the player
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch(powerupID)
                {
                case 0:
                    player.TripleShotActive(); // Activate triple shot on player
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                    player.ShieldsActive();
                    break;
                default:
                    Debug.Log("Deafult Value");
                    break;
                }
            }

            // Destroy the power-up after activation
            Destroy(this.gameObject);
        }
    }
}
