using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator EndTextAnim;

    [SerializeField] GameObject ResultUI;
    [SerializeField] GameObject EndText;

    [SerializeField] private GameObject ResultText;

    [SerializeField] private GameObject OutGameObject; 

    private bool _flag;

    void Start()
    {
        HideResultUI();
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

        // 子オブジェクトを全て非表示にしておく
        foreach (Transform child in ResultText.transform)
        {
            child.gameObject.SetActive(false);
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

        foreach (Transform child in ResultText.transform)
        {
            yield return new WaitForSeconds(1f);
            child.gameObject.SetActive(true);
        }
    }
}
