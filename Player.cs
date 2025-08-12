using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this namespace to use TextMeshPro
using UnityEngine.UI; // Add this for UI elements like Image

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    
    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShot;
    private float _speedMultiplier = 2f;

    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool _isSpeedBoostActive = false;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    // Scoring system variables
    [SerializeField]
    private int _score = 0; // Variable to track player's score
    [SerializeField]
    private TMP_Text _scoreText;
     // Use TMP_Text for TextMeshPro elements

     [SerializeField]
     private AudioClip _laserSoundClip;
     private AudioSource _audioSource;

     [SerializeField]
     private GameObject _leftEngine, _rightEngine;

    // UI Manager reference for managing lives (hearts)
    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is null");
        }

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("audio source on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
        // Initialize score display
        _score = 0;
        UpdateScoreText();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // Handle movement based on whether speed boost is active
        if (!_isSpeedBoostActive)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }

        // Clamping player movement in the Y-axis
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        // Wrapping player movement in the X-axis
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.075f, 0), Quaternion.identity);
        }
        _audioSource.Play();
    }

    public void Damage()
{
    if (_isShieldActive == true)
    {
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
        return;
    }

    _lives -= 1;

    if(_lives ==2)
    {
        _leftEngine.SetActive(true);
    }
    else if(_lives ==1)
    {
        _rightEngine.SetActive(true);
    }

    _uiManager.UpdateLives(_lives);

    if (_lives < 1)
    {
        _uiManager.ShowGameOver(); 
        _spawnManager.OnPlayerDeath();
        Destroy(this.gameObject, 0.1f);
    }
}

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        Debug.Log("Triple shot activated");

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
        while(_isSpeedBoostActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _isSpeedBoostActive = false;
            _speed /= _speedMultiplier; 
        }
    }

    public void ShieldsActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        Debug.Log("Shield Activated!");
    }

    // Method to update the score and UI text
    public void AddScore(int points)
    {
        _score += points; // Increment the score by the points passed in
        UpdateScoreText(); // Update the UI text
    }

    // Update the score text UI
    private void UpdateScoreText()
    {
        _scoreText.text = "Score: " + _score.ToString();
    }
}
