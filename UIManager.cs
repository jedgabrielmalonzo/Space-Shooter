using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this to handle UI Image
using TMPro; // Add this for TextMeshPro

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Image> hearts; // Assign the heart images in the Inspector
    [SerializeField] private TMP_Text gameOverText; // Reference to the Game Over TextMeshPro object

    // This method updates the heart display
    public void UpdateLives(int lives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < lives)
            {
                hearts[i].enabled = true; // Show heart if lives are remaining
            }
            else
            {
                hearts[i].enabled = false; // Hide heart if the player lost a life
            }
        }
    }

    // Method to show Game Over text
    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true); // Activate the Game Over text
    }
}
