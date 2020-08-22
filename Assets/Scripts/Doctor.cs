using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : MonoBehaviour
{
	public int AnimationChoice = 1;
	public bool FlippedSprite = false;

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Animator>()?.SetInteger("Choice", AnimationChoice);
		GetComponentInChildren<SpriteRenderer>().flipX = FlippedSprite;
    }
}
