using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextDataBase", menuName = "TextDataBase")]
public class TextDataBase : ScriptableObject
{
    [SerializeField, Header("テキスト")]
    public TextData[] textSet;
}

[System.Serializable]
public class TextData
{
    public int id;
    [TextArea(2, 5)] public string textData;
}
