using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InGameManager : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] private Text timerText;        // タイマー表示
    [SerializeField] private Text scoreText;        // スコア表示
    [SerializeField] private Text rankingText;      // ランキング表示
    [SerializeField] private Button tapButton;      // クリック用ボタン
    [SerializeField] private Button startButton;    // ゲーム開始ボタン

    [Header("ポップアップ")]
    [SerializeField] private GameObject resultPanel; // ゲーム終了時のパネル
    [SerializeField] private Text resultScoreText;   // 今回のスコア表示用

    [Header("ゲーム設定")]
    [SerializeField] private float gameTime = 10f;  // 制限時間（秒）

    private float currentTime;
    private int score = 0;

    // ランキング関連
    private List<int> highScores = new List<int>();
    private const int rankingMax = 5; // 上位5件

    private bool isGameActive = false;

    void Start()
    {
        // 初期化
        currentTime = gameTime;
        score = 0;
        UpdateUI();

        // ボタンに処理を登録
        tapButton.onClick.AddListener(() => AddScore(1));
        startButton.onClick.AddListener(StartGame);

        // 最初はタップボタンとポップアップ無効
        tapButton.interactable = false;
        resultPanel.SetActive(false);

        // ハイスコア読み込み & 表示
        LoadHighScores();
        ShowRanking();
    }

    void Update()
    {
        if (isGameActive)
        {
            // タイマー更新
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                EndGame();
            }
            UpdateUI();
        }
    }

    public void AddScore(int value)
    {
        if (!isGameActive) return; // ゲーム中のみ加算
        score += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + currentTime.ToString("F1");
        scoreText.text = "Score: " + score.ToString();
    }

    void StartGame()
    {
        isGameActive = true;
        currentTime = gameTime;
        score = 0;

        tapButton.interactable = true;    // タップ可能に
        startButton.interactable = false; // スタートボタン無効化
        resultPanel.SetActive(false);     // 前回の結果を隠す

        UpdateUI();
    }

    void EndGame()
    {
        isGameActive = false;
        tapButton.interactable = false;   // タップできなくする
        startButton.interactable = true;  // 再挑戦できるようにする

        SaveHighScore(score);
        ShowRanking();

        // ✅ ポップアップに今回のスコアを表示
        resultScoreText.text = "今回のスコア: " + score.ToString();
        resultPanel.SetActive(true);
    }

    void SaveHighScore(int newScore)
    {
        highScores.Add(newScore);
        highScores.Sort((a, b) => b.CompareTo(a)); // 降順
        if (highScores.Count > rankingMax)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }

        // PlayerPrefsに保存
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadHighScores()
    {
        highScores.Clear();
        for (int i = 0; i < rankingMax; i++)
        {
            if (PlayerPrefs.HasKey("HighScore" + i))
            {
                highScores.Add(PlayerPrefs.GetInt("HighScore" + i));
            }
        }
    }

    void ShowRanking()
    {
        rankingText.text = "Ranking\n";
        for (int i = 0; i < highScores.Count; i++)
        {
            rankingText.text += (i + 1) + "位: " + highScores[i] + "\n";
        }
    }
}
