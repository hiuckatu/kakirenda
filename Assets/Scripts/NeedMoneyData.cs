using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeedMoneyDataBase", menuName = "NeedMoneyDataBase")]
public class NeedMoneyData : ScriptableObject
{
    [SerializeField] public int[] needMoneyByLevel; // 各レベルに必要な金額
}
