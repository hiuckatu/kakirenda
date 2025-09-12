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
    [SerializeField] private TextMeshProUGUI scoreText_Result;
    [SerializeField] private TextMeshProUGUI moneyText_Result;

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

    [Header("CountText")]
    [SerializeField] private GameObject CountTextPrefab;
    [SerializeField] private Transform CountTextParent;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;

    private float timeRemaining;
    private float score;
    private bool isPlaying = false;

    private float scoreRatio;
    private float moneyRatio;
    private float CriticalRatio;
    private float AutoClicktimeRatio;
    private float AddTime;
    private float LuckValue;

    private float AutoClicktime;
    private float InGamemoney = 0;
    [SerializeField, Range(0f, 1f)] private float coinChance;

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

            AutoClicktime += Time.deltaTime;
            if (AutoClicktime >= 1.0f / (1f + AutoClicktimeRatio))
            {
                AddScore();
                AutoClicktime = 0f;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            StartEffect.SetActive(false);
        }
    }

    private void StartGame()
    {
        var data = jsonManager.LoadData();

        scoreRatio = data.level_score * 0.15f;
        moneyRatio = data.level_money * 0.10f;
        CriticalRatio = data.level_critical * 0.12f;
        AutoClicktimeRatio = 0.323f * Mathf.Log(data.level_autoClick + 1);
        AddTime = data.level_time * 0.2f;
        LuckValue = data.level_luck * 0.01f;

        score = 0;
        InGamemoney = 0;
        AutoClicktime = 0f;
        timeRemaining = gameTime + AddTime;
        isPlaying = true;

        tapButton.interactable = true;

        audioManager.StartGameSound();

        InGameObject.SetActive(true);
        OutGameObject.SetActive(false);
        StartEffect.SetActive(true);
        UpdateUI();
    }

    private void EndGame()
    {
        isPlaying = false;
        InGameObject.SetActive(false);
        tapButton.interactable = false;
        scoreText_Result.text = scoreText.text;
        moneyText_Result.text = moneyText.text;
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
            float addValue = 1.0f + scoreRatio;
            bool isCritical = false;

            if (Random.value < 0.15f + (LuckValue / 10f))
            {
                addValue *= (2.0f + CriticalRatio);
                isCritical = true;
            }

            score += addValue;

            EatOysterAnimation();
            UpdateScore();

            SpawnCountText("+" + addValue.ToString("F0"), isCritical);

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
        if (Random.value < (coinChance + LuckValue / 10f))
        {
            float addValue = 1f + moneyRatio;
            bool isCritical = false;

            if (Random.value < 0.15f + (LuckValue / 10f))
            {
                addValue *= (2.0f + CriticalRatio);
                isCritical = true;
            }

            InGamemoney += addValue;
            UpdateMoneyUI();

            SpawnCountText("Coin +" + addValue.ToString("F0"), isCritical, Color.yellow);
        }
    }

    private void BlackBandAnimation()
    {
        animator.SetTrigger("StartEffectTrigger");
    }

    private void UpdateTimer()
    {
        timerText.text = "残り時間: " + Mathf.Max(0, timeRemaining).ToString("F2") + "秒";
    }

    private void UpdateScore()
    {
        scoreText.text = "スコア: " + score.ToString("F0");
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "獲得コイン: " + InGamemoney.ToString("F0");
    }

    private void EatOysterAnimation()
    {
        animator_human.SetTrigger("EatingTrigger");
    }

    private void SpawnCountText(string text, bool isCritical, Color? overrideColor = null)
    {
        GameObject obj = Instantiate(CountTextPrefab, CountTextParent);
        RectTransform rect = obj.GetComponent<RectTransform>();

        float offsetX = Random.Range(-Screen.width * 0.25f, Screen.width * 0.25f);
        float offsetY = Random.Range(-Screen.height * 0.2f, Screen.height * 0.2f);
        rect.anchoredPosition = new Vector2(offsetX, offsetY);

        CountTextController dmgText = obj.GetComponent<CountTextController>();

        if (overrideColor.HasValue)
        {
            dmgText.SetText(text, overrideColor.Value);
            rect.localScale = isCritical ? Vector3.one * 1.3f : Vector3.one;
        }
        else
        {
            if (isCritical)
            {
                dmgText.SetText(text, Color.red);
                rect.localScale = Vector3.one * 1.3f;
            }
            else
            {
                dmgText.SetText(text, Color.black);
                rect.localScale = Vector3.one;
            }
        }
    }
}
