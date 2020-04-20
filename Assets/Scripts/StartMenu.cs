using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text highscoreText;
    public RectTransform heartTransform;
    public GameObject pressSpaceGameObject;

    // Start is called before the first frame update
    void Start()
    {
        var highScore = PlayerPrefs.GetInt("Highscore");
        highscoreText.text = $"High score: {highScore}";

        LeanTween.scale(pressSpaceGameObject, Vector3.one * 1.3f, 1.0f)
            .setLoopPingPong();

        var initialPos = heartTransform.position;
        LeanTween.move(heartTransform, new Vector2(0,1f), 1.0f).setEaseOutBounce()
            .setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }
}
