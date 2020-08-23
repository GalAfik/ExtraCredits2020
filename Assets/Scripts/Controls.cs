using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controls : MonoBehaviour
{
	public Image Press;
	public Image Hold;
	public TMP_Text HoldLabel;

    // Start is called before the first frame update
    void Start()
    {
		Press.enabled = false;
		Hold.enabled = false;
		HoldLabel.enabled = false;
	}

    public void ShowPress(bool show) 
	{
		Press.enabled = show;
	}

	public void ShowHold(bool show)
	{
		Hold.enabled = show;
		HoldLabel.enabled = show;
	}
}
