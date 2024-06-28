using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform playerTransform;
    public TextMeshProUGUI countdownText;
    private Vector3 startingPosition;
    private int countdownTime = 30; // Countdown starting time
    private int currentCountdownTime;
    private Coroutine countdownCoroutine;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // Record the player's starting position
        startingPosition = playerTransform.position;
        // Initialize the countdown timer
        currentCountdownTime = countdownTime;
        // Start the countdown timer when the game begins
        countdownCoroutine = StartCoroutine(CountdownTimer());
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has fallen (e.g., if the player's y position is less than a given limit)
        if (playerTransform.position.y < -10f) // Example limit, adjust as necessary
        {
            // If the player falls, call Player.Respawn() to put the player back to its start point
            playerTransform.GetComponent<PlayerMovement>().Respawn(startingPosition);
        }

        // Check for spacebar input to pause or resume the timer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                // Resume the countdown
                isPaused = false;
                countdownCoroutine = StartCoroutine(CountdownTimer());
            }
            else
            {
                // Pause the countdown
                isPaused = true;
                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                    countdownCoroutine = null;
                }
            }
        }
    }
    // Tick method
    private IEnumerator CountdownTimer()
    {
        while (currentCountdownTime > 0)
        {
            countdownText.text = currentCountdownTime.ToString(); // Update the UI text
            yield return new WaitForSeconds(1); // Wait for 1 second
            currentCountdownTime--;
        }

        countdownText.text = "0"; // Display 0 when the timer ends
        Debug.Log("Game Over"); // Output "Game Over" in the Console
    }
}
