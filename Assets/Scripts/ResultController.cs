using System.Collections;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator EndTextAnim;
    [SerializeField] Animator BackHomeButonAnim;

    [SerializeField] GameObject ResultUI;
    [SerializeField] GameObject EndText;
    [SerializeField] GameObject BackHomeButton;

    [SerializeField] private GameObject[] ResultObjects; // 順番に表示したいオブジェクトを配列で指定

    [SerializeField] private GameObject OutGameObject;


    void Start()
    {
        OnClickBackHomeButton();
    }

    void Update()
    {
        if (EndTextAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            EndText.activeSelf)
        {
            EndText.SetActive(false);
            ShowResult();
        }
    }

    public void HideResultUI()
    {
        ResultUI.SetActive(false);
        EndText.SetActive(false);
        BackHomeButton.SetActive(false);

        foreach (GameObject obj in ResultObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ShowEndEffect()
    {
        EndText.SetActive(true);
        EndTextAnim.SetTrigger("EndEffectTrigger");
    }

    private void ShowResult()
    {
        ResultUI.SetActive(true);
        animator.SetTrigger("ResultTrigger");

        StartCoroutine(WaitForResultAnimation());
    }

    private IEnumerator WaitForResultAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        for (int i = 0; i < ResultObjects.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            ResultObjects[i].SetActive(true);

            if (i == ResultObjects.Length - 1)
            {
                ResultButtonAnim();
            }
        }
    }

    private void ResultButtonAnim()
    {
        BackHomeButton.SetActive(true);
        BackHomeButonAnim.SetTrigger("BackHomeTrigger");
    }

    public void OnClickBackHomeButton()
    {
        HideResultUI();
        OutGameObject.SetActive(true);
    }
}
