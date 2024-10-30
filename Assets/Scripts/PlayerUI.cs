using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private Button _rockButton; // Button for the Rock choice
    private Button _paperButton; // Button for the Paper choice
    private Button _scissorButton; // Button for the Scissor choice

    private void Start()
    {
        // Finding and assigning buttons from the UI hierarchy
        _rockButton = transform.Find("RockBtn").GetComponent<Button>(); // Find the Rock button
        _rockButton.onClick.AddListener(() => PlayerButtonClick(1)); // Assign click event for Rock
        
        _paperButton = transform.Find("PaperBtn").GetComponent<Button>(); // Find the Paper button
        _paperButton.onClick.AddListener(() => PlayerButtonClick(2)); // Assign click event for Paper
        
        _scissorButton = transform.Find("ScissorBtn").GetComponent<Button>(); // Find the Scissor button
        _scissorButton.onClick.AddListener(() =>PlayerButtonClick(3)); // Assign click event for Scissor
        
        // Subscribe to GameManager events to handle round state changes
        GameManager.Instance.OnRoundEnded += ExitPlayMode; // Exit play mode when the round ends
        GameManager.Instance.OnRoundStarted += EnterPlayMode; // Enter play mode when the round starts
    }

    private void PlayerButtonClick(int choice)
    {
        GameManager.Instance.GetPlayerChoice(choice);
        AudioManager.Instance.Play("Click");
    }
    
    // Method to enable player buttons when entering play mode
    private void EnterPlayMode()
    {
        // Enable buttons to allow player interaction
        _rockButton.interactable = true;
        _paperButton.interactable = true;
        _scissorButton.interactable = true;
    }

    // Method to disable player buttons when exiting play mode
    private void ExitPlayMode(Choice playerChoice, Choice systemChoice)
    {
        // Disable buttons to prevent further player interaction during animations and result reveal
        _rockButton.interactable = false;
        _paperButton.interactable = false;
        _scissorButton.interactable = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe from GameManager events to prevent memory leaks
        GameManager.Instance.OnRoundEnded -= ExitPlayMode; // Remove exit play mode listener
        GameManager.Instance.OnRoundStarted -= EnterPlayMode; // Remove enter play mode listener
    }
    
}
