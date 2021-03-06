﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Text;
using System;
using UnityEngine.Video;

public class QuestionManager : MonoBehaviour {

	public TextAsset bancoDePreguntas;
	public GameObject pregunta;
	public Canvas canvas;

	public VideoClip clip;
	public string gender;

	private const string videoPath = "Assets/Videos/{0}/{1}.mp4";

	private KeywordRecognizer reconocedorDeVoz;

	[SerializeField]
	private string[] palabrasClaves;

	private DictationRecognizer dictationRecognizer;

	private GameObject camera;
	private VideoPlayer videoPlayer;
	private AudioSource audioSource;

	void Start()
	{
		camera = GameObject.Find("Main Camera");
		videoPlayer = camera.AddComponent<VideoPlayer>();
		audioSource = gameObject.AddComponent<AudioSource>();
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
		videoPlayer.playOnAwake = false;
		videoPlayer.source = VideoSource.Url;
		gender = "Male";

		dictationRecognizer = new DictationRecognizer ();
		dictationRecognizer.DictationHypothesis += (text) => {
			print(text);	
		};

		dictationRecognizer.DictationResult += (text, confidence) => {
			print(text  + " - " + confidence);	
		};
		dictationRecognizer.Start ();

		palabrasClaves = new string[]{"Historia", "Fundado", "Francia"};
		reconocedorDeVoz = new KeywordRecognizer(palabrasClaves);
		reconocedorDeVoz.OnPhraseRecognized += OnPhraseRecognized;

		reconocedorDeVoz.Start ();

		videoPlayer.loopPointReached += CheckOver;
	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		videoPlayer.enabled = true;
		// Set mode to Audio Source.
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		// We want to control one audio track with the video player
		videoPlayer.controlledAudioTrackCount = 1;
		// We enable the first track, which has the id zero
		videoPlayer.EnableAudioTrack(0, true);
		// ...and we set the audio source for this track
		videoPlayer.SetTargetAudioSource(0, audioSource);
		videoPlayer.url = String.Format(videoPath, gender, args.text);

		StartCoroutine (prepareAndPlayVideo ());
	}

	private IEnumerator prepareAndPlayVideo()
	{
		videoPlayer.Prepare ();

		while (!videoPlayer.isPrepared) {
			yield return new WaitForEndOfFrame();
		}

		videoPlayer.Play();
	}

	void CheckOver(UnityEngine.Video.VideoPlayer vp)
	{
		videoPlayer.enabled = false;
	}
}
