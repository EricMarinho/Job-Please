using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobsButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameObject jobPaperCanvas;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonPressed);
        button.interactable = false;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        AudioManager.instance.PlayPaperSound();
        if (jobPaperCanvas.activeSelf)
        {
            GameManager.instance.OnJobsPaperButtonExit?.Invoke();
            return;
        }

        GameManager.instance.OnJobsPaperButtonPressed?.Invoke();
    }
}
