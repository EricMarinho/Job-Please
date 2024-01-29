using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagens : MonoBehaviour
{
    bool isInteractable = false;
    [SerializeField] private float delayTime = 1f;

    public static Personagens instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        GameManager.instance.OnInterviewerEnterAnimationEnd += ()=> 
        {
            isInteractable = true;
        };
        GameManager.instance.OnInterviewEnd += () =>
        {
            isInteractable = false;
        };
    }

    public void Interact()
    {
        StartCoroutine(DelayToInteract());
    }

    public IEnumerator DelayToInteract()
    {
        Debug.Log("DelayToInteract");
        isInteractable = false;
        yield return new WaitForSeconds(delayTime);
        isInteractable = true;
    }

    private void OnMouseDown()
    {
        if (!isInteractable) return;

        TelaDialogo.Instancia.Exibir();
    }
}
