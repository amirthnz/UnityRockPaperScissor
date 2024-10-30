using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

// Enum representing the choices available in the game
public enum Choice
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}
public class GameManager : MonoBehaviour
{
    
    // Local Variables
    private Choice _playerChoice; // Stores the player's choice
    private Choice _systemChoice; // Stores the system's choice
    private int _playerScore; // Player's score
    private int _systemScore; // System's score
    private int _neededPoint; // Points needed to win the game
    
    // Events (Observers) to notify other classes about game state changes
    public event Action<Choice, Choice> OnRoundEnded; // Triggered when a round ends, providing player and system choices
    public event Action<int, int> OnScoresUpdated; // Triggered when scores are updated
    public event Action<string> OnGameEnded; // Triggered when the game ends, providing the winner
    public event Action<int> OnGameStarted; // Triggered when a new game session starts, providing points needed
    public event Action OnRoundStarted; // Triggered when a new round starts
    
    public static GameManager Instance { get; private set; } // Singleton instance of GameManager

    private void Awake()
    {
        Instance = this; // Assign this instance to the static Instance variable
    }

    private void Start()
    {
        _playerScore = 0; // Initialize player score
        _systemScore = 0; // Initialize system score
    }

    // Start a new session 
    // This method is called by start buttons
    public void StartSession(int pointneeded)
    {
        _neededPoint = pointneeded; // Set the points needed to win
        OnGameStarted?.Invoke(_neededPoint); // Notify that the game has started
    }

    // Store the player's choice and start the round by taking the system's choice
    public void GetPlayerChoice(int choice)
    {
        _playerChoice = (Choice)choice; // Convert the choice to the Choice enum
        PlayRound(); // Start the round
    }

    // Take the system's choice and calculate the round winner
    private void PlayRound()
    {
        _systemChoice = (Choice)Random.Range(1, 4); // Random choice for the system (1 to 3)
        DetermineWinner(); // Determine the winner of the round
    }
    
    private void DetermineWinner()
    {
        // Basic logic to determine the winner
        if (_playerChoice == _systemChoice)
        {
        }
        else if ((_playerChoice == Choice.Rock && _systemChoice == Choice.Scissors) ||
                 (_playerChoice == Choice.Paper && _systemChoice == Choice.Rock) ||
                 (_playerChoice == Choice.Scissors && _systemChoice == Choice.Paper))
        {
            _playerScore++; // Increment player's score
        }
        else
        {
            _systemScore++; // Increment system's score
        }
        
        // Notify observers that the round has ended with player and system choices to show animation and update UI
        OnRoundEnded?.Invoke(_playerChoice, _systemChoice);
    }
    
    // This method is called by the animation handler 
    public void EndRound()
    {
        // Notify observers about the updated scores
        OnScoresUpdated?.Invoke(_playerScore, _systemScore);
        // Check if any player reached the maximum points to end the game
        if (CheckEndGame())
        {
            // Notify that the game has ended and provide the winner
            OnGameEnded?.Invoke(_playerScore > _systemScore ? "Player" : "System");
        }
        else
        {
            // Notify that a new round has started
            OnRoundStarted?.Invoke();
        }
    }
    
    // Check if any player has reached the maximum points
    private bool CheckEndGame()
    {
        // Return true if either player has reached the needed points
        return _playerScore >= _neededPoint || _systemScore >= _neededPoint;
    }
    
}
