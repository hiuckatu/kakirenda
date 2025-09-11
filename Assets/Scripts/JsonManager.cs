using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using UnityEngine;

public class JsonManager : MonoBehaviour
{

    private string fileName = "SaveData.json";

    private string FilePath
    {
        get
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "StreamingAssets", fileName);
#else
            return Path.Combine(Application.dataPath, "StreamingAssets", fileName);
#endif
        }
    }

    public void Start()
    {
        CreateInitialJsonFile();
    }

    public void CreateInitialJsonFile()
    {
        if (File.Exists(FilePath)) return;

        GameSaveData saveData = new GameSaveData
        {
            money = 0,
            MaxScore = 0,
            TotalScore = 0,

            level_money = 0,
            level_score = 0,
            level_critical = 0,
            level_autoClick = 0,
            level_time = 0,
            level_luck = 0,
        };

        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(FilePath, json);
        Debug.Log("初期JSONファイルを作成しました: " + FilePath);
    }

    public GameSaveData LoadData()
    {
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("JSONファイルが見つかりません。初期化します。");
            CreateInitialJsonFile();
        }
        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public void SaveData(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
        Debug.Log("データを保存しました。");
    }

    // money所持数加減算
    public void AddItemCount(int addCount)
    {
        var data = LoadData();
        data.money = Mathf.Max(0, data.money + addCount);
        SaveData(data);
    }

    // money所持数上書き
    public void SetItemCount(int count)
    {
        var data = LoadData();
        data.money = Mathf.Max(0, count);
        SaveData(data);
    }

    // ---- データ定義 ----

    [Serializable]
    public class GameSaveData
    {
        public int money;

        public float MaxScore;
        public float TotalScore;

        public int level_money;
        public int level_score;
        public int level_critical;
        public int level_autoClick;
        public int level_time;
        public int level_luck;
    }
}