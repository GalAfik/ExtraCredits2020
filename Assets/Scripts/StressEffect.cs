using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StressEffect : MonoBehaviour
{
	// Volume
	private Volume Volume;

	// Bloom
	private Bloom Bloom;
	private float BloomIntensity;

	// Vignette
	private Vignette Vignette;
	private float VignetteIntensity;

	// Color Adjustments
	private ColorAdjustments ColorAdjustments;
	private float ColorAdjustmentSaturation;

	// Lens Distortion
	private LensDistortion LensDistortion;
	private float LensDistortionIntensity;

	// Chromatic Abberation
	private ChromaticAberration ChromaticAberration;
	private float ChromaticAberrationIntensity;

	// Start is called before the first frame update
	void Start()
	{
		Volume = GetComponent<Volume>();
		Volume.profile.TryGet(out Bloom);
		Volume.profile.TryGet(out Vignette);
		Volume.profile.TryGet(out ColorAdjustments);
		Volume.profile.TryGet(out LensDistortion);
		Volume.profile.TryGet(out ChromaticAberration);

		// Get the initial values
		BloomIntensity = Bloom.intensity.value;
		VignetteIntensity = Vignette.intensity.value;
		ColorAdjustmentSaturation = ColorAdjustments.saturation.value;
		LensDistortionIntensity = LensDistortion.intensity.value;
		ChromaticAberrationIntensity = ChromaticAberration.intensity.value;
	}

	public void SetStressEffect(float stress, float maxStress)
	{
		// Set the effects
		Bloom.intensity.value = Remap(stress, 0, maxStress, BloomIntensity, 50);
		Vignette.intensity.value = Remap(stress, 0, maxStress, VignetteIntensity, 0.6f);
		ColorAdjustments.saturation.value = Remap(stress, 0, maxStress, ColorAdjustmentSaturation, -100);
		LensDistortion.intensity.value = Remap(stress, 0, maxStress, LensDistortionIntensity, -0.8f);
		ChromaticAberration.intensity.value = Remap(stress, 0, maxStress, ChromaticAberrationIntensity, 1);
	}

	private float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
