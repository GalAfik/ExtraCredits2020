using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StressEffect : MonoBehaviour
{
	private Volume Volume;
	private Bloom Bloom;
	private Vignette Vignette;
	private ColorAdjustments ColorAdjustments;
	private LensDistortion LensDistortion;

	// Start is called before the first frame update
	void Start()
	{
		Volume = GetComponent<Volume>();
		Volume.profile.TryGet(out Bloom);
		Volume.profile.TryGet(out Vignette);
		Volume.profile.TryGet(out ColorAdjustments);
		Volume.profile.TryGet(out LensDistortion);
	}

	public void SetStressEffect(float stress, float maxStress)
	{
		// Set the effects
		Bloom.intensity.value = Remap(stress, 0, maxStress, 1, 20);
		Vignette.intensity.value = Remap(stress, 0, maxStress, 0.25f, 1);
		ColorAdjustments.saturation.value = Remap(stress, 0, maxStress, 0, -100);
		LensDistortion.intensity.value = Remap(stress, 0, maxStress, 0, -0.7f);
	}

	private float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
