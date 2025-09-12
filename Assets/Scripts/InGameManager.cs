using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class ScoreAttackGame : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Button")]
    [SerializeField] private Button tapButton;
    [SerializeField] private Button startButton;

    [Header("参照オブジェクト")]
    [SerializeField] private GameObject InGameObject;
    [SerializeField] private GameObject OutGameObject;
    [SerializeField] private GameObject StartEffect;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator_human;
    
    [Header("Json")]
    [SerializeField] private JsonManager jsonManager;

    [Header("Game Settings")]
    [SerializeField] private float gameTime;

    [Header("ResultScripts")]
    [SerializeField] private ResultController resultController;

    private float timeRemaining;
    private float score;
    private bool isPlaying = false;

    // 追加：所持コイン
    private int InGamemoney = 0;
    [SerializeField, Range(0f, 1f)] private float coinChance = 0.28f;

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

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            StartEffect.SetActive(false);
        }
    }

    private void StartGame()
    {
        score = 0;
        InGamemoney = 0;
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
        resultController.ShowEndEffect();
        jsonManager.AddValue(JsonManager.SaveDataType.Money, InGamemoney);
        jsonManager.AddValue(JsonManager.SaveDataType.TotalScore, score);
        if (jsonManager.LoadData().MaxScore <= score)
        {
            jsonManager.SetValue(JsonManager.SaveDataType.MaxScore, score);
        }
    }

    private void AddScore()
    {
        if (isPlaying)
        {
            score += 1;
            EatOysterAnimation();
            UpdateScore();
            MoneyByDraw();
        }
    }

    private void UpdateUI()
    {
        UpdateTimer();
        UpdateScore();
        UpdateMoneyUI();
        BlackBandAnimation();
    }

    private void MoneyByDraw()
    {
        if (Random.value < coinChance)
        {
            InGamemoney += 1;
            UpdateMoneyUI();
        }
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

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Coins: " + InGamemoney;
    }

    private void EatOysterAnimation()
    {
        animator_human.SetTrigger("EatingTrigger");
    }
}
