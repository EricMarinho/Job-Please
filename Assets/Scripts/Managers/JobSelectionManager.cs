using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobSelectionManager : MonoBehaviour
{

    [SerializeField] private Button signButton;
    [SerializeField] private Button changeJobButton;
    [SerializeField] private Image imgJobPage;
    private List<JobsPaperData> currentLevelPossibleJobs = new();
    private int currentJobIndex = 0;

    private void Awake()
    {
        GameManager.instance.OnJobsPaperButtonPressed += () =>
        {
            imgJobPage.sprite = currentLevelPossibleJobs[currentJobIndex].jobSprite;
        };

        currentLevelPossibleJobs.Clear();
        GameManager.instance.OnStageInitialize += ResetJobsList;
        GameManager.instance.OnJobsPaperButtonPressed += ShowJobsList;
        signButton.onClick.AddListener(SignPage);
        changeJobButton.onClick.AddListener(NextPage);
    }

    private void ShowJobsList()
    {
        signButton.gameObject.SetActive(true);

        if (currentLevelPossibleJobs.Count <= 1)
        {
            changeJobButton.gameObject.SetActive(false);
            return;
        }
        changeJobButton.gameObject.SetActive(true);
    }

    private void NextPage()
    {
        currentJobIndex++;
        if (currentJobIndex ==  currentLevelPossibleJobs.Count)
        {
            currentJobIndex = 0;
        }

        imgJobPage.sprite = currentLevelPossibleJobs[currentJobIndex].jobSprite;
        AudioManager.instance.PlayPaperSound();
    }

    private void SignPage()
    {
        signButton.gameObject.SetActive(false);
        changeJobButton.gameObject.SetActive(false);
        JobsAssignedManager.instance.AssignJob(currentLevelPossibleJobs[currentJobIndex]);
        currentLevelPossibleJobs.RemoveAt(currentJobIndex);
        GameManager.instance.OnDialogueEnd = null;
        GameManager.instance.OnDialogueEnd += () => GameManager.instance.OnInterviewEnd?.Invoke();
        TelaDialogo.Instancia.Exibir(false);

        AudioManager.instance.PlaySignSound();
        currentJobIndex = 0;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnStageInitialize -= ResetJobsList;
        GameManager.instance.OnJobsPaperButtonPressed -= ShowJobsList;
        signButton?.onClick.RemoveListener(SignPage);
    }

    private void ResetJobsList()
    {
        currentLevelPossibleJobs.Clear();
        foreach (JobsPaperData job in GameManager.instance.GetPossibleJobs())
        {
            currentLevelPossibleJobs.Add(job);
        }
    }
}
