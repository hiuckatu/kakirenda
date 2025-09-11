using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class OpeningController : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private float fadeDuration;   // フェード時間
    [SerializeField] private float displayTime;    // 表示時間
    [SerializeField] private float skipTime;       // スキップ可能になるまでの時間
    [SerializeField] private TextDataBase textDataBase; // ScriptableObject参照
    [SerializeField] private Animator animator;         // 任意のアニメーション用
    [SerializeField] private string triggerName = "PlayAnim";
    [SerializeField] private string nextSceneName;      // 遷移先のシーン名
    [SerializeField] private TMP_Text skipText;         // 「左クリックでスキップ」表示用

    private int currentIndex = 0;
    private float timer = 0f;

    private enum State { FadeIn, Display, FadeOut, Idle }
    private State state = State.Idle;

    private Color transparentColor;
    private Color opaqueColor;

    // --- 追加 ---
    private float sceneTimer = 0f;
    private bool canSkip = false;

    private void Start()
    {
        if (textDataBase == null || textDataBase.textSet.Length == 0) return;

        // 最初のテキストをセット
        textMeshPro.text = textDataBase.textSet[currentIndex].textData;

        transparentColor = textMeshPro.color;
        transparentColor.a = 0f;
        opaqueColor = textMeshPro.color;
        opaqueColor.a = 1f;

        textMeshPro.color = transparentColor;
        state = State.FadeIn;
        timer = 0f;

        if (skipText != null) skipText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // --- シーン開始からの時間をカウント ---
        sceneTimer += Time.deltaTime;

        if (!canSkip && sceneTimer >= skipTime)
        {
            canSkip = true;
            if (skipText != null) skipText.gameObject.SetActive(true);
        }

        // --- 左クリックでシーン遷移 ---
        if (canSkip && Input.GetMouseButtonDown(0))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        // --- テキスト処理 ---
        if (state == State.Idle) return;

        timer += Time.deltaTime;
        float t;

        switch (state)
        {
            case State.FadeIn:
                // 通常テキストだけフェードイン
                t = Mathf.Clamp01(timer / fadeDuration);
                textMeshPro.color = Color.Lerp(transparentColor, opaqueColor, t);

                if (t >= 1f)
                {
                    state = State.Display;
                    timer = 0f;
                }
                break;

            case State.Display:
                if (timer >= displayTime)
                {
                    // 最後のテキストならフェードアウトせずに終了
                    if (currentIndex == textDataBase.textSet.Length - 1)
                    {
                        state = State.Idle; // テキスト残す
                    }
                    else
                    {
                        state = State.FadeOut;
                        timer = 0f;
                    }
                }
                break;

            case State.FadeOut:
                t = Mathf.Clamp01(timer / fadeDuration);
                textMeshPro.color = Color.Lerp(opaqueColor, transparentColor, t);

                if (t >= 1f)
                {
                    currentIndex++;
                    if (currentIndex < textDataBase.textSet.Length)
                    {
                        textMeshPro.text = textDataBase.textSet[currentIndex].textData;

                        // --- 最後のテキスト ---
                        if (currentIndex == textDataBase.textSet.Length - 1)
                        {
                            textMeshPro.color = opaqueColor; // フェードインせず即表示
                            state = State.Display;
                            timer = 0f;

                            // アニメーション再生
                            if (animator != null && !string.IsNullOrEmpty(triggerName))
                            {
                                animator.SetTrigger(triggerName);
                            }
                        }
                        // --- 通常のテキスト ---
                        else
                        {
                            textMeshPro.color = transparentColor;
                            state = State.FadeIn;
                            timer = 0f;
                        }
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }
                break;
        }
    }
}
