using System.Collections.Generic;
using UnityEngine;

public class HandAnimations : MonoBehaviour
{
    
    private readonly int _bounce = Animator.StringToHash("Bounce"); // Hash for the Bounce animation trigger

    public List<Sprite> choiceSprites; // List to hold Rock, Paper, Scissors sprites
    public SpriteRenderer playerHandSprite; // SpriteRenderer for the player's hand
    public SpriteRenderer systemHandSprite; // SpriteRenderer for the system's hand

    private int _playerChoiceIndex; // Index of the player's choice in the choiceSprites list
    private int _systemChoiceIndex; // Index of the system's choice in the choiceSprites list
    
    private Animator _animator; // Animator component for handling animations

    private void Start()
    {
        _animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject

        // Subscribe to the OnRoundEnded event from GameManager to trigger animations
        GameManager.Instance.OnRoundEnded += PlayAnimations;
    }
    
    private void PlayAnimations(Choice playerChoice, Choice systemChoice)
    {
        // Set default sprite for both hands at the start of the round
        playerHandSprite.sprite = choiceSprites[0];
        systemHandSprite.sprite = choiceSprites[0];
        
        // Set selected choices based on the player's and system's choices
        _playerChoiceIndex = (int)playerChoice; // Convert player's choice to an index
        _systemChoiceIndex = (int)systemChoice; // Convert system's choice to an index
        
        // Play the bounce animation
        PlayBounceAnimation();
    }
    
    private void PlayBounceAnimation()
    {
        // Trigger the bounce animation
        _animator.SetTrigger(_bounce);
        AudioManager.Instance.Play("Wave"); // Play a wave sound effect
    }

    public void EndAnimation()
    {
        // Set the Animator to the "Steady" state to end the animation
        _animator.Play("Steady");
        AudioManager.Instance.Play("HandStop"); // Play a sound effect to indicate the end of the hand animation
        GameManager.Instance.EndRound(); // Notify the GameManager that the round has ended
        // Update the sprites to reflect the player's and system's choices
        playerHandSprite.sprite = choiceSprites[_playerChoiceIndex - 1];
        systemHandSprite.sprite = choiceSprites[_systemChoiceIndex - 1];
    }
    
}
