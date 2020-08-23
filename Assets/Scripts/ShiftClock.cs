using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShiftClock : MonoBehaviour
{
	public TMP_Text Label;

	public void SetTime(float time)
	{
		int minutes = Mathf.RoundToInt(time) / 60;
		int seconds = Mathf.RoundToInt(time) % 60;
		Label.SetText(minutes.ToString("D2") + ":" + seconds.ToString("D2"));
	}
}
