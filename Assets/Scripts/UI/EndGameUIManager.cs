using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup endGameCanvasGroup;
    [SerializeField] private Button nextFeedbackButton;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text feedbackText;
    private int currentFeedback = 1;

    private void Start()
    {
        nextFeedbackButton.enabled = true;
        nextFeedbackButton.onClick.AddListener(OnNextFeedbackButtonClick);
        SetFeedbackData();
    }

    private void SetFeedbackData()
    {
        characterPortrait.sprite = JobsAssignedManager.instance.assignedJobs[currentFeedback].characterSprite;
        characterName.SetText(JobsAssignedManager.instance.assignedJobs[currentFeedback].characterName);
        feedbackText.SetText(JobsAssignedManager.instance.assignedJobs[currentFeedback].feedback);
        //Do Something if the job is correct
    }

    private void OnNextFeedbackButtonClick()
    {
        Debug.Log("NextFeedback");
        nextFeedbackButton.enabled = false;
        currentFeedback++;
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            if (currentFeedback < JobsAssignedManager.instance.assignedJobs.Count)
            {
                SetFeedbackData();
                canvasGroup.DOFade(1f, 0.5f).OnComplete(() => nextFeedbackButton.enabled = true);
            }
            else
            {
                Debug.Log("Real EndGame");
                endGameCanvasGroup.gameObject.SetActive(true);
                endGameCanvasGroup.DOFade(1f, 0.5f);
            }
        });
    }


}
