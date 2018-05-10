using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSounds : MonoBehaviour
{
	AudioSource audioSource = null;
	public AudioClip impactClip = null;

	void Start()
	{
		// Add an AudioSource component and set up some defaults
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.spatialize = true;
		audioSource.spatialBlend = 1.0f;
		audioSource.dopplerLevel = 0.0f;
		audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		audioSource.maxDistance = 20f;

        
		// Load the Sphere sounds from the Resources folder
		//impactClip = Resources.Load<AudioClip>("Impact");

	}

	// Occurs when this object starts colliding with another object
	void OnCollision(Collision collision)
	{
        Debug.Log("sound played");
        audioSource.clip = impactClip;
			audioSource.Play();
        
	}
		
}