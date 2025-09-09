using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreAttackGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;       // タイマー
    [SerializeField] private TextMeshProUGUI scoreText;       // スコア
    [SerializeField] private Button tapButton;         // 連打ボタン
    [SerializeField] private Button startButton;       // スタートボタン
    [SerializeField] private GameObject InGameObject;

    [Header("Game Settings")]
    [SerializeField] private float gameTime;     // 制限時間

    private float timeRemaining;
    private int score;
    private bool isPlaying = false;

    void Start()
    {
        tapButton.onClick.AddListener(AddScore);
        startButton.onClick.AddListener(StartGame);
        tapButton.interactable = false; 
        InGameObject.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (isPlaying)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                EndGame();
            }
            UpdateTimer();
        }
    }

    private void StartGame()
    {
        score = 0;
        timeRemaining = gameTime;
        isPlaying = true;

        tapButton.interactable = true;
        InGameObject.SetActive(true);
        UpdateUI();
    }

    private void EndGame()
    {
        isPlaying = false;
        InGameObject.SetActive(false);
        tapButton.interactable = false;
    }

    private void AddScore()
    {
        if (isPlaying)
        {
            score += 1;
            UpdateScore();
        }
    }

    private void UpdateUI()
    {
        UpdateTimer();
        UpdateScore();
    }

    private void UpdateTimer()
    {
        timerText.text = "Time: " + Mathf.Max(0, timeRemaining).ToString("F1");
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
