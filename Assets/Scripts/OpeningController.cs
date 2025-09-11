using UnityEngine;
using TMPro;

public class OpeningController : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private float fadeDuration = 1f;   // フェード時間
    [SerializeField] private float displayTime = 1.5f;  // 表示時間
    [SerializeField] private TextDataBase textDataBase; // ScriptableObject参照
    [SerializeField] private Animator animator;         // 任意のアニメーション用
    [SerializeField] private string triggerName = "PlayAnim"; // Animatorのトリガー名

    private int currentIndex = 0;
    private float timer = 0f;

    private enum State { FadeIn, Display, FadeOut, Idle }
    private State state = State.Idle;

    private Color transparentColor;
    private Color opaqueColor;

    private void Start()
    {
        if (textDataBase == null || textDataBase.textSet.Length == 0) return;

        textMeshPro.text = textDataBase.textSet[currentIndex].textData;
        transparentColor = textMeshPro.color;
        transparentColor.a = 0f;
        opaqueColor = textMeshPro.color;
        opaqueColor.a = 1f;

        textMeshPro.color = transparentColor;
        state = State.FadeIn;
        timer = 0f;
    }

    private void Update()
    {
        if (state == State.Idle) return;

        timer += Time.deltaTime;
        float t;

        switch (state)
        {
            case State.FadeIn:
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
                    state = State.FadeOut;
                    timer = 0f;
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
                        state = State.FadeIn;
                        timer = 0f;
                    }
                    else
                    {
                        // 最後のテキストが終わったタイミングでアニメーション
                        if (animator != null && !string.IsNullOrEmpty(triggerName))
                        {
                            animator.SetTrigger(triggerName);
                        }

                        state = State.Idle;
                    }
                }
                break;
        }
    }
}
