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
    [SerializeField] private GameObject Buttonprefab;
    [SerializeField] private GameObject ShopPanel;

    [SerializeField] private JsonManager jsonManager;

    private void Start()
    {
        CreateButton();
    }

    public void OpenShopButton()
    {
        ShopPanel.SetActive(true);
    }

    private void CreateButton()
    {
        var obj = Instantiate(Buttonprefab,ParentRectTransForm);
    }
}
