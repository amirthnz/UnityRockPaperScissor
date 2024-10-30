using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Start game")]
    public GameObject startGamePanel; // Panel shown at the start of the game
    public Button startThreeRoundButton; // Button to start a 3-round game
    public Button startFiveRoundButton; // Button to start a 5-round game
    
    [Space(2)]
    [Header("End game")]
    public GameObject gameOverPanel; // Panel shown when the game is over
    public Button playAgainButton; // Button to play again
    public TextMeshProUGUI winnerText; // Text to display the winner's name
    
    [Space(2)]
    [Header("In game")]
    public List<Image> playerCirclesList; // List of player score indicators
    public List<Image> systemCirclesList; // List of system score indicators
    public Transform playerPointHolder; // Parent transform for player score indicators
    public Transform systemPointHolder; // Parent transform for system score indicators
    public Image imagePrefab; // Prefab for score indicator images
    public Sprite emptyCircleSprite; // Sprite for the empty circle
    public Sprite filledCircleSprite; // Sprite for the filled circle
    
    private int _playerCurrentScore; // Current score of the player
    private int _systemCurrentScore; // Current score of the system
    

    private void Start()
    {
        // Hide the game over panel at the start
        gameOverPanel.SetActive(false);
        startGamePanel.SetActive(true);
        
        // Subscribe to GameManager events
        GameManager.Instance.OnScoresUpdated += UpdateScoreDisplay; // Update score display when scores are updated
        GameManager.Instance.OnGameEnded += DisplayWinner; // Display the winner when the game ends
        GameManager.Instance.OnGameStarted += StartGame; // Start game when the game starts
        
        // Add listeners to buttons
        playAgainButton.onClick.AddListener(PlayAgain); // Play again button functionality
        startThreeRoundButton.onClick.AddListener(() => StartSession(3)); // Start 3-round game
        startFiveRoundButton.onClick.AddListener(() => StartSession(5)); // Start 5-round game
        
        
    }

    private void StartSession(int pointNeeded)
    {
        // Start a new game session with the specified points needed to win
        GameManager.Instance.StartSession(pointNeeded);
        AudioManager.Instance.Play("Click"); // Play click sound
    }

    private void StartGame(int pointNeeded)
    {
        // Initialize the game when it starts
        CreatePointBar(pointNeeded); // Create score indicators
        UpdateScoreUI(); // Update the score UI to reflect initial scores
        startGamePanel.SetActive(false); // Hide the start game panel
        AudioManager.Instance.Play("Start"); // Play start sound
    }
    
    private void CreatePointBar(int pointNeeded)
    {
        // Create score indicators for the player
        for (var i = 0; i < pointNeeded; i++)
        {
            var image = Instantiate(imagePrefab, playerPointHolder); // Instantiate a new image for the player
            var rectTransform = image.GetComponent<RectTransform>(); // Get the RectTransform of the image
            rectTransform.localPosition = new Vector3(i * 72, 0, 0); // Position the image in a horizontal line
            playerCirclesList.Add(image); // Add the image to the player circles list
        }
        
        // Create score indicators for the system
        for (var i = 0; i < pointNeeded; i++)
        {
            var image = Instantiate(imagePrefab, systemPointHolder); // Instantiate a new image for the system
            var rectTransform = image.GetComponent<RectTransform>(); // Get the RectTransform of the image
            rectTransform.localPosition = new Vector3(i * -72f, 0, 0); // Position the image in a horizontal line
            systemCirclesList.Add(image); // Add the image to the system circles list
        }
    }
    
    private void UpdateScoreDisplay(int playerScore, int systemScore)
    {
        // Update the current scores based on the received values
        if (_playerCurrentScore != playerScore)
        {
            AudioManager.Instance.Play("PlayerScored"); // Play player scored sound
        } else if (_systemCurrentScore != systemScore)
        {
            AudioManager.Instance.Play("SystemScored"); // Play player scored sound
        }
        _playerCurrentScore = playerScore;
        _systemCurrentScore = systemScore;
        UpdateScoreUI(); // Update the UI to reflect the new scores
    }
    
    private void UpdateScoreUI()
    {
        // Update the player score indicators based on the current score
        for (var i = 0; i < playerCirclesList.Count; i++)
        {
            playerCirclesList[i].sprite = i < _playerCurrentScore ? filledCircleSprite : // Set to filled circle if the score is higher than the index
                emptyCircleSprite; // Set to empty circle otherwise
        }
        
        // Update the system score indicators based on the current score
        for (var i = 0; i < systemCirclesList.Count; i++)
        {
            systemCirclesList[i].sprite = i < _systemCurrentScore ? filledCircleSprite : // Set to filled circle if the score is higher than the index
                emptyCircleSprite; // Set to empty circle otherwise
        }
    }

    private void DisplayWinner(string winner)
    {
        // Play a sound based on the winner
        AudioManager.Instance.Play(winner == "Player" ? "Win" : "Lose");
        StartCoroutine(EndGame(winner)); // Start the end game coroutine
    }

    private IEnumerator EndGame(string winner)
    {
        // Wait for 1.5 seconds before displaying the game over panel
        yield return new WaitForSeconds(1.5f); 
        gameOverPanel.SetActive(true); // Show the game over panel
        
        winnerText.text = winner + " Wins!"; // Set the winner text
    }

    private void OnDestroy()
    {
        // Unsubscribe from GameManager events
        GameManager.Instance.OnScoresUpdated -= UpdateScoreDisplay;
        GameManager.Instance.OnGameEnded -= DisplayWinner;
        GameManager.Instance.OnGameStarted -= StartGame;
    }

    private void PlayAgain()
    {
        // Reload the current scene to play again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.Instance.Play("Click"); // Play click sound
    }
}
