using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Transform chairTransform;
    [SerializeField] private Transform characterSpriteTransform;
    private Vector3 characterSpriteStartPosition;

    [Header("Test")]
    [SerializeField] private InterviewData currentInterview;

    private void Start()
    {
        characterSpriteStartPosition = characterSpriteTransform.position;
        GameManager.instance.OnInterviewStart += MoveInToStartPos;
        GameManager.instance.OnInterviewEnd += MoveOutFromStartPos;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnInterviewStart -= MoveInToStartPos;
        GameManager.instance.OnInterviewEnd -= MoveOutFromStartPos;
    }

    private void MoveInToStartPos()
    {
        currentInterview = GameManager.instance.GetCurrentInterview();
        characterSpriteTransform.DOMove(chairTransform.position, currentInterview.enterAnimationDuration).SetEase(currentInterview.enterAnimationType).OnComplete(() => GameManager.instance.OnInterviewerEnterAnimationEnd?.Invoke());
    }

    private void MoveOutFromStartPos()
    {
        characterSpriteTransform.DOMove(characterSpriteStartPosition, currentInterview.leaveAnimationDuration).SetEase(currentInterview.leaveAnimationType).OnComplete(() => GameManager.instance.OnInterviewerLeaveAnimationEnd?.Invoke()); ;
    }

    [Button("Restart Enter Animation")]
    public void RestartEnterAnimation()
    {
        characterSpriteTransform.position = characterSpriteStartPosition;
        characterSpriteTransform.DOMove(chairTransform.position, currentInterview.enterAnimationDuration).SetEase(currentInterview.enterAnimationType);
    }

    [Button("Restart Leave Animation")]
    public void RestartLeaveAnimation()
    {
        characterSpriteTransform.position = chairTransform.position;
        characterSpriteTransform.DOMove(characterSpriteStartPosition, currentInterview.leaveAnimationDuration).SetEase(currentInterview.leaveAnimationType);

    }

}
