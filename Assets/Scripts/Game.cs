using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering.PostProcessing;

public class Game : MonoBehaviour
{
    [HideInInspector]
    public static Game instance;
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    [Header("Game Pieces")]
    public GameObject player;
    public GameObject enemy;
    public GameObject ball;
    [Header("Sounds")]
    public AudioClip ballHitClip;
    public AudioClip loseClip;
    public AudioSource backgroundMusic;
    [Header("Effects")]
    public PostProcessVolume postProcessVolume;
    public TrailRenderer ballTrailRenderer;
    public ParticleSystem ballParticleSystem;
    public ParticleSystem backgroundParticleSystem;
    private Vignette vignette;
    private ChromaticAberration chromaticAbberation;
    private Color initialBackgroundColor;
    

    private AudioSource audioSource;
    private int score = 0;
    private bool canReceive = false;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        vignette = postProcessVolume.profile.GetSetting<Vignette>();
        chromaticAbberation = postProcessVolume.profile.GetSetting<ChromaticAberration>();
        initialBackgroundColor = Camera.main.backgroundColor;

        UpdateHighscore();
        RestartMatch();
    }

    private void UpdateHighscore()
    {
        var highScore = PlayerPrefs.GetInt("Highscore");
        highscoreText.text = $"High score: {highScore}";
    }

    private void RestartMatch()
    {
        score = 0;
        scoreText.text = score.ToString();

        ResetEffects();

        canReceive = false;

        var pos = player.transform.position;
        ball.transform.position = pos + new Vector3(0, 0, 1);

        ball.GetComponent<Ball>().Direction = Vector3.forward;
    }

    public void Enemy_BallHit()
    {
        audioSource.clip = ballHitClip;
        audioSource.Play();

        canReceive = true;
    }

    public void Player_BallHit()
    {
        if (!canReceive)
            return;

        audioSource.clip = ballHitClip;
        audioSource.Play();

        IncrementScore();

        canReceive = false;

        enemy.GetComponent<Enemy>().UpdateLocation();
        UpdateEffects();
    }

    public void Ball_OutOfBounds(Vector3 position)
    {
        //audioSource.clip = loseClip;
        //audioSource.Play();

        var highScore = PlayerPrefs.GetInt("Highscore");
        if (score > highScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            UpdateHighscore();
        }

        RestartMatch();
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = score.ToString();

        scoreText.gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
        LeanTween.scale(scoreText.gameObject, new Vector3(1f, 1f, 1f), 1f).setEaseOutBounce();
    }

    private void ResetEffects()
    {
        ballTrailRenderer.Clear();
        ballTrailRenderer.enabled = false;

        Camera.main.backgroundColor = initialBackgroundColor;
        //vignette.intensity.value = 0f;
        LeanTween.value(backgroundMusic.gameObject, (n) => backgroundMusic.volume = n, backgroundMusic.volume, 0, 0.1f).setOnComplete(() => backgroundMusic.Stop());
        chromaticAbberation.intensity.value = 0f;

        ballParticleSystem.Stop();
        ballParticleSystem.Clear();

        backgroundParticleSystem.Stop();
        backgroundParticleSystem.Clear();

        //ball.GetComponent<Ball>().IsWobbly = true;
    }

    public void UpdateEffects()
    {
        if (score == 1)
        {
            ballTrailRenderer.enabled = true;
        }

        if (score < 10)
        {
            Color.RGBToHSV(Camera.main.backgroundColor, out float h, out float s, out float v);
            if (v > 0)
            {
                LeanTween.value(Camera.main.gameObject, (n) => Camera.main.backgroundColor = Color.HSVToRGB(h, s, n, false), v, v - 0.05f, 1f);
            }
        }
        
        if (score > 4)
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
                LeanTween.value(backgroundMusic.gameObject, (n) => backgroundMusic.volume = n, 0f, 0.1f, 1f);
            }
            else
            {
                if (backgroundMusic.volume < 0.7f)
                {
                    LeanTween.value(backgroundMusic.gameObject, (n) => backgroundMusic.volume = n, backgroundMusic.volume, backgroundMusic.volume + 0.1f, 1f);
                }
            }
        }

        if (score > 7)
        {
            if (chromaticAbberation.intensity.value < 1.0f)
            {
                LeanTween.value(postProcessVolume.gameObject, (n) => chromaticAbberation.intensity.value = n, chromaticAbberation.intensity.value, chromaticAbberation.intensity.value + 0.1f, 1f);
            }
        }

        if (score == 17)
        {
            if (ballParticleSystem.isStopped)
            {
                ballParticleSystem.Play();
            }
        }

        if (score > 24)
        {
            Color.RGBToHSV(Camera.main.backgroundColor, out float h, out float s, out float v);
            if (v < 0.7f)
            {
                LeanTween.value(Camera.main.gameObject, (n) => Camera.main.backgroundColor = Color.HSVToRGB(1.0f, 1.0f, n, false), v, v + 0.05f, 1f);
            }
        }

        if (score == 35)
        {
            if (backgroundParticleSystem.isStopped)
            {
                backgroundParticleSystem.Play();
            }
        }
    }
}
