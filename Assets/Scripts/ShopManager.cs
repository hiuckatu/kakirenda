using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject ShopPanel;
    [SerializeField] private NeedMoneyData needMoneyData;
    [SerializeField] private JsonManager jsonManager;

    [SerializeField] private ShopItemUI[] shopItemUIs;

    [Header("UI表示用")]
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        ShopPanel.SetActive(false);
        UpdateAllShopUI();
        UpdateMoneyText();
    }

    public void OpenShopButton()
    {
        ShopPanel.SetActive(true);
        UpdateMoneyText();
    }

    public void CloseShopButton() => ShopPanel.SetActive(false);

    public void LevelUpBycategory(int category)
    {
        var data = jsonManager.LoadData();
        int currentLevel = GetLevelByCategory(data, category);

        if (currentLevel >= needMoneyData.needMoneyByLevel.Length)
        {
            Debug.Log("これ以上レベルアップできません (最大レベル)");
            return;
        }

        int needMoney = needMoneyData.needMoneyByLevel[currentLevel];

        if (data.money >= needMoney)
        {
            data.money -= needMoney;
            SetLevelByCategory(data, category, currentLevel + 1);
            jsonManager.SaveData(data);

            Debug.Log($"カテゴリ {category} をレベル {currentLevel + 1} にアップ！ 残金: {data.money}");

            UpdateShopUI(category);
            UpdateMoneyText();
        }
        else
        {
            Debug.Log("お金が足りません！");
        }
    }

    private int GetLevelByCategory(JsonManager.GameSaveData data, int category)
    {
        return category switch
        {
            0 => data.level_money,
            1 => data.level_score,
            2 => data.level_critical,
            3 => data.level_autoClick,
            4 => data.level_time,
            5 => data.level_luck,
            _ => 0
        };
    }

    private void SetLevelByCategory(JsonManager.GameSaveData data, int category, int newLevel)
    {
        switch (category)
        {
            case 0: data.level_money = newLevel; break;
            case 1: data.level_score = newLevel; break;
            case 2: data.level_critical = newLevel; break;
            case 3: data.level_autoClick = newLevel; break;
            case 4: data.level_time = newLevel; break;
            case 5: data.level_luck = newLevel; break;
        }
    }

    // --- UI更新 ---
    private void UpdateAllShopUI()
    {
        var data = jsonManager.LoadData();
        for (int i = 0; i < shopItemUIs.Length; i++)
        {
            UpdateShopUI(i, data);
        }
    }

    private void UpdateShopUI(int category, JsonManager.GameSaveData data = null)
    {
        if (data == null) data = jsonManager.LoadData();
        int currentLevel = GetLevelByCategory(data, category);

        if (currentLevel >= needMoneyData.needMoneyByLevel.Length)
        {
            shopItemUIs[category].costText.text = "MAX";
        }
        else
        {
            int needMoney = needMoneyData.needMoneyByLevel[currentLevel];
            shopItemUIs[category].costText.text = $"費用: {needMoney} コイン";
        }

        // --- カテゴリごとに表示を変更 ---
        string effectText = category switch
        {
            0 => $"Lv.{currentLevel}  +{currentLevel * 10f}%",           // money
            1 => $"Lv.{currentLevel}  +{currentLevel * 5f}%",            // score
            2 => $"Lv.{currentLevel}  +{currentLevel * 12f}%",           // critical
            3 => $"Lv.{currentLevel}  +{(1f + 0.323f * Mathf.Log(currentLevel + 1)).ToString("F2")}",    // autoClick
            4 => $"Lv.{currentLevel}  +{currentLevel * 0.08f}秒",        // time
            5 => $"Lv.{currentLevel}  +{currentLevel * 1f}%",            // luck
            _ => $"Lv.{currentLevel}"
        };

        shopItemUIs[category].levelText.text = effectText;
    }

    // --- 所持金表示更新 ---
    private void UpdateMoneyText()
    {
        var data = jsonManager.LoadData();
        moneyText.text = $"所持金: {data.money} コイン";
    }
}
