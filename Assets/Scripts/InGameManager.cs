using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class ScoreAttackGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;       
    [SerializeField] private TextMeshProUGUI scoreText;       

    [SerializeField] private Button tapButton;         
    [SerializeField] private Button startButton;       

    [SerializeField] private GameObject InGameObject;
    [SerializeField] private GameObject OutGameObject;
    [SerializeField] private GameObject StartEffect;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator_human;

    [Header("Game Settings")]
    [SerializeField] private float gameTime;     

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
        StartEffect.SetActive(false);
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

        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            StartEffect.SetActive(false);
        }
    }

    private void StartGame()
    {
        score = 0;
        timeRemaining = gameTime;
        isPlaying = true;

        tapButton.interactable = true;
        InGameObject.SetActive(true);
        OutGameObject.SetActive(false);
        StartEffect.SetActive(true);

        UpdateUI();
    }

    private void EndGame()
    {
        isPlaying = false;
        InGameObject.SetActive(false);
        OutGameObject.SetActive(true);
        tapButton.interactable = false;
    }

    private void AddScore()
    {
        if (isPlaying)
        {
            score += 1;
            EatOyster();
            UpdateScore();
        }
    }

    private void UpdateUI()
    {
        UpdateTimer();
        UpdateScore();
        BlackBandAnimation();
    }

    private void BlackBandAnimation()
    {
        animator.SetTrigger("StartEffectTrigger");
    }

    private void UpdateTimer()
    {
        timerText.text = "Time: " + Mathf.Max(0, timeRemaining).ToString("F1");
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    private void EatOyster()
    {
        animator_human.SetTrigger("EatingTrigger");
    }
}
