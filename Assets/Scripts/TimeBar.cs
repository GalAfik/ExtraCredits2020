using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class TimeBar : MonoBehaviour
{
	public Slider Slider;

	public void SetMaxTime(float time)
	{
		Slider.maxValue = time;
		Slider.value = 0;
	}

	public void SetTime(float time)
	{
		Slider.value = time;
	}
}
