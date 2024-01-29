using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int errorCount { get; private set; } = 0;

    private int currentLevelIndex = 0;
    private int currentInterviewIndex = 0;

    [SerializeField] private List<LevelData> levelDataList = new();

    public static GameManager instance;

    [SerializeField] private CanvasGroup endGameCanvas;
    [SerializeField] private float endGameCanvasFadeInScreenTime = 1f;

    //Actions
    public Action OnStageInitialize;
    public Action OnInterviewStart;
    public Action OnInterviewerEnterAnimationEnd;
    public Action OnInterviewEnd;
    public Action OnInterviewerLeaveAnimationEnd;
    public Action OnStageEnd;

    //UI Actions
    public Action OnJobsPaperButtonPressed;
    public Action OnJobsPaperButtonExit;

    public Action OnDialogueStart;
    public Action OnDialogueEnd;
    public Action OnEndDialogueSentence;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        endGameCanvas.gameObject.SetActive(false);
        OnInterviewerLeaveAnimationEnd += NextInterview;
        InitializeStage();               
    }

    private void InitializeStage()
    {
        Debug.Log("InitializeStage");
        OnStageInitialize?.Invoke();
    }

    private void NextStage()
    {
        currentInterviewIndex = 0;
        InitializeStage();
    }

    private void NextInterview()
    {
        currentInterviewIndex++;
        if (currentInterviewIndex >= levelDataList[currentLevelIndex].interviewDataList.Count)
        {
            currentLevelIndex++;
            if (currentLevelIndex < levelDataList.Count)
            {
                OnStageEnd?.Invoke();
                NextStage();
            }
            else
            {
                endGameCanvas.alpha = 0f;
                endGameCanvas.gameObject.SetActive(true);
                endGameCanvas.DOFade(1f, endGameCanvasFadeInScreenTime);
                Debug.Log("EndGame");
            }

            return;
        }

        OnInterviewStart?.Invoke();
    }

    public string GetLevelTittle() => levelDataList[currentLevelIndex].levelName;

    public List<JobsPaperData> GetPossibleJobs() => levelDataList[currentLevelIndex].possibleJobs;

    public InterviewData GetCurrentInterview() => levelDataList[currentLevelIndex].interviewDataList[currentInterviewIndex];
}
