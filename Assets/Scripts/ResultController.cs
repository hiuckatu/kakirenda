using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator animator2;

    [SerializeField] GameObject ResultUI;
    [SerializeField] GameObject EndText;

    private bool _flag;
    void Start()
    {
        HideResultUI();
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            EndText.SetActive(false);
            ShowResult();
        }
    }

    public void HideResultUI()
    {
        ResultUI.SetActive(false);
        EndText.SetActive(false);
    }

    public void ShowEndEffect()
    {
        EndText.SetActive(true);
        animator2.SetTrigger("EndEffcetTrigger");
    }

    private void ShowResult()
    {
        ResultUI.SetActive(true);
        animator.SetTrigger("ResultTrigger");

    }
}
