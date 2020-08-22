using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Emote : MonoBehaviour
{
	// Componenets
	private Animator Animator;
	private SpriteRenderer SpriteRenderer;

	// Sprites
	public Sprite[] Emotes;

	// Label
	public TMP_Text Label;

    // Start is called before the first frame update
    void Start()
    {
		// Get components
		Animator = GetComponent<Animator>();
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Display(int emoteNumber, int labelNumber = 0)
	{
		if (Emotes[emoteNumber] != null)
		{
			// Switch the sprite to the currently specified emote
			SpriteRenderer.sprite = Emotes[emoteNumber];

			// Play the Display animation
			Animator?.SetTrigger("Display");

			// If displaying the blank emote, display and change the label
			if (emoteNumber == 0)
			{
				Label.enabled = true;
				Label.SetText(labelNumber.ToString());
			}
			// Otherwise, hide the label
			else
			{
				Label.enabled = false;
			}
		}
	}
}
