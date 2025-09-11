using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private RectTransform ParentRectTransForm;
    [SerializeField] private GameObject ShopPanel;

    [SerializeField] private GameObject CoinShop;
    [SerializeField] private GameObject ScoreShop;
    [SerializeField] private GameObject CriticalShop;
    [SerializeField] private GameObject AutoClickShop;
    [SerializeField] private GameObject TimeShop;
    [SerializeField] private GameObject LuckShop;


    [SerializeField] private JsonManager jsonManager;

    private void Start()
    {
        ShopPanel.SetActive(false);
    }

    public void OpenShopButton()
    {
        ShopPanel.SetActive(true);
    }
    
    public void CloseShopButton()
    {
        ShopPanel.SetActive(false);
    }

    

}
