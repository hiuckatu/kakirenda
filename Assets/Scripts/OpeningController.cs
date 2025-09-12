using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OpeningController : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float displayTime;
    [SerializeField] private float skipTime;
    [SerializeField] private TextDataBase textDataBase; 
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "PlayAnim";
    [SerializeField] private string nextSceneName;
    [SerializeField] private TMP_Text skipText;

    [SerializeField] private AudioManager audioManager;

    private int currentIndex = 0;
    private float timer = 0f;

    private enum State { FadeIn, Display, FadeOut, Idle }
    private State state = State.Idle;

    private Color transparentColor;
    private Color opaqueColor;

    private float sceneTimer = 0f;
    private bool canSkip = false;

    private void Start()
    {
        if (textDataBase == null || textDataBase.textSet.Length == 0) return;

        audioManager.PlayOpeningBGM();

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
        sceneTimer += Time.deltaTime;

        if (!canSkip && sceneTimer >= skipTime)
        {
            canSkip = true;
            if (skipText != null) skipText.gameObject.SetActive(true);
        }

        if (canSkip && Input.GetMouseButtonDown(0))
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

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
                    if (currentIndex == textDataBase.textSet.Length - 1)
                    {
                        state = State.Idle;
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

                        if (currentIndex == textDataBase.textSet.Length - 1)
                        {
                            textMeshPro.color = opaqueColor;
                            state = State.Display;
                            timer = 0f;

                            if (animator != null && !string.IsNullOrEmpty(triggerName))
                            {
                                audioManager.PlayOpeningSound();
                                animator.SetTrigger(triggerName);
                            }
                        }
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
