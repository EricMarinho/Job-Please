using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level Data", order = 0)]
public class InterviewData : ScriptableObject
{
    public string characterName;
    public List<GameObject> interviewItemList = new List<GameObject>();
    public PossibleJobs correctJob;
    [Header("Dialogue")]
    public Dialogo startDialogue;
    public Dialogo endDialogue;
    [Header("Sprites")]
    public Sprite normalCharacterSprite;
    public Sprite talkingCharacterSprite;
    public Sprite endCharacterSprite;
    public Sprite resumeSprite;
    [Header("Animation")]
    public Ease enterAnimationType;
    [Range(0.1f, 3f)]
    public float enterAnimationDuration;
    public Ease leaveAnimationType;
    [Range(0.1f, 3f)]
    public float leaveAnimationDuration;
    [Header("Audio")]
    public AudioClip music;
    public AudioClip talkSound;
}
