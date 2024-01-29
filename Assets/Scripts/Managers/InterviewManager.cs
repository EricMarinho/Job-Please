using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnInterviewerEnterAnimationEnd += InitializeDialogue;
    }
    private void OnDestroy()
    {
        GameManager.instance.OnInterviewerEnterAnimationEnd -= InitializeDialogue;
    }

    private void InitializeDialogue()
    {
        TelaDialogo.Instancia.Exibir();
    }

}
