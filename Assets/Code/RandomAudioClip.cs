using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums
public enum AudioType
{
	None,
	Angry,
	Happy,
	Eating,
	Hungry,
	Satisfied,
	Toothbrush,
	Combing,
	Pincer,
	NailFiling,
}
#endregion

[RequireComponent(typeof(AudioSource))]
public class RandomAudioClip : MonoBehaviour
{
	public AudioClip[] audioClips;

	private AudioSource audioSource;

	// Use this for initialization
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayRandomAudioClip()
	{
		audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
		audioSource.Play();
	}
}