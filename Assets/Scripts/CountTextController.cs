using TMPro;
using UnityEngine;

public class CountTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float moveSpeed;   
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float fadeDuration = 0.5f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
    }

    public void SetText(string text, Color color)
    {
        damageText.text = text;
        damageText.color = color;
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer > lifeTime - fadeDuration)
        {
            float t = (lifeTime - timer) / fadeDuration;
            canvasGroup.alpha = Mathf.Clamp01(t);
        }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}