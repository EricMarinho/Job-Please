using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using NaughtyAttributes;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float delayForFadingOut = 1.5f;
    [SerializeField] private float fadeOutScreenTime = 0.5f;
    [SerializeField] private float fadeInScreenTime = 0.5f;

    [SerializeField] private GameObject jobsPaperObj;
    [SerializeField] private GameObject jobsPaperButtonObj;
    [SerializeField] private RectTransform jobsPaperSpriteRect;
    [SerializeField] private Button jobsPaperButton;
    [SerializeField] private Sprite jobsSprite;

    [SerializeField] private TMP_Text signNameText;

    [SerializeField] private CanvasGroup transitionScreen;
    [SerializeField] protected TMP_Text stageTittle;

    private List<GameObject> currentSpawnedInterviewItems = new();
    private List<float> currentSpawnedInterviewItemsPosition = new();

    private void Start()
    {
        Debug.Log("UIManager Start");
        FadeInScreen();
        //ResetDialogueEndEvents();
        GameManager.instance.OnInterviewStart += ResetDialogueEndEvents;
        GameManager.instance.OnStageInitialize += CallFadeOutScreen;
        GameManager.instance.OnStageEnd += FadeInScreen;
        GameManager.instance.OnJobsPaperButtonPressed += ShowJobsPaper;
        GameManager.instance.OnJobsPaperButtonExit += HideJobsPaper;
        GameManager.instance.OnDialogueStart += StopJobsButtonInteraction;
        GameManager.instance.OnDialogueEnd += RestartJobsButtonInteraction;
        GameManager.instance.OnInterviewerLeaveAnimationEnd += ClearInterviewItems;
        transitionScreen.gameObject.SetActive(true);
        jobsPaperObj.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnInterviewStart -= ResetDialogueEndEvents;
        GameManager.instance.OnStageInitialize -= CallFadeOutScreen;
        GameManager.instance.OnStageEnd -= FadeInScreen;
        GameManager.instance.OnJobsPaperButtonPressed -= ShowJobsPaper;
        GameManager.instance.OnJobsPaperButtonExit -= HideJobsPaper;
        GameManager.instance.OnDialogueStart -= StopJobsButtonInteraction;
        GameManager.instance.OnDialogueEnd -= RestartJobsButtonInteraction;

    }

    private void ShowJobsPaper()
    {
        signNameText.SetText(GameManager.instance.GetCurrentInterview().characterName);
        foreach (GameObject interviewItem in currentSpawnedInterviewItems)
        {
            interviewItem.transform.DOMoveY(10f, 0.3f);
        }
        
        jobsPaperButton.image.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.3f).OnComplete(() => {
            jobsPaperButton.image.sprite = GameManager.instance.GetCurrentInterview().resumeSprite;
            jobsPaperButton.image.rectTransform.DORotate(new Vector3(0f, 0f, 17f), 0.3f);
            jobsPaperObj.SetActive(true);
            jobsPaperSpriteRect.DOAnchorPosY(20f, 0.3f);
        });
    }

    private void HideJobsPaper()
    {
        int i = 0;
        jobsPaperSpriteRect.DOAnchorPosY(-1200f, 0.3f).OnComplete(() => jobsPaperObj.SetActive(false));
        jobsPaperButton.image.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.3f).OnComplete(() =>
        {
            jobsPaperButton.image.sprite = jobsSprite;
            jobsPaperButton.image.rectTransform.DORotate(new Vector3(0f, 0f, 17f), 0.3f);
            
            foreach (GameObject interviewItem in currentSpawnedInterviewItems)
            {
                interviewItem.transform.DOMoveY(currentSpawnedInterviewItemsPosition[i], 0.3f);
                i++;
            }
        });
    }

    private void CallFadeOutScreen()
    {
        StartCoroutine(FadeOutScreenDelayed());
    }

    private void StopJobsButtonInteraction()
    {
        jobsPaperButton.interactable = false;
    }

    private void RestartJobsButtonInteraction()
    {
        jobsPaperButton.interactable = true;
    }

    private IEnumerator FadeOutScreenDelayed()
    {
        yield return new WaitForSeconds(delayForFadingOut);
        FadeOutScreen();
    }

    private void SpawnInterviewItem()
    {
        GameManager.instance.OnDialogueEnd = null;
        GameManager.instance.OnDialogueEnd += RestartJobsButtonInteraction;
        RestartJobsButtonInteraction();
        int i = 0;
        foreach (GameObject interviewItem in GameManager.instance.GetCurrentInterview().interviewItemList)
        {
           
            currentSpawnedInterviewItems.Add(Instantiate(interviewItem));
            currentSpawnedInterviewItemsPosition.Add(currentSpawnedInterviewItems[i].transform.position.y);
            i++;
        };
    }

    private void ClearInterviewItems()
    {
        GameManager.instance.OnJobsPaperButtonExit?.Invoke();
        if (currentSpawnedInterviewItems.Count > 0)
        {
            foreach (GameObject interviewItem in currentSpawnedInterviewItems)
            {
                Destroy(interviewItem);
            }
            currentSpawnedInterviewItems.Clear();
        }
    }

    public void ResetDialogueEndEvents()
    {
        GameManager.instance.OnDialogueEnd = null;
        GameManager.instance.OnDialogueEnd += SpawnInterviewItem;
    }

    [Button("FadeIn")]
    public void FadeInScreen()
    {
        stageTittle.SetText(GameManager.instance.GetLevelTittle());
        transitionScreen.DOFade(1f, fadeInScreenTime);
    }

    [Button("FadeOut")]
    private void FadeOutScreen()
    {
        Debug.Log("FadeOutScreen");
        transitionScreen.DOFade(0f, fadeOutScreenTime);
        GameManager.instance.OnInterviewStart?.Invoke();
    }
}
