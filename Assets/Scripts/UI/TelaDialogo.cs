using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TelaDialogo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textoNomePersonagem;

    [SerializeField]
    private TextMeshProUGUI textoFalaDialogo;

    [SerializeField]
    private float intervaloTempoEntreLetrasEmSegundos;

    private bool isSpeaking = false;

    private Coroutine preencherTextoCoroutine;
    private Dialogo dialogo;

    private AudioClip characterVoice;

    private static TelaDialogo instancia;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        Esconder();
    }

    public static TelaDialogo Instancia
    {
        get 
        {
            return instancia;
        }
    }
    public void Exibir(bool isStartDialogue = true)
    {
        
        characterVoice = GameManager.instance.GetCurrentInterview().talkSound;

        if(isSpeaking)
        {
            Avancar();
            return;
        }

        isSpeaking = true;
        InterviewData currentInterview = GameManager.instance.GetCurrentInterview();
        dialogo = isStartDialogue ? currentInterview.startDialogue : currentInterview.endDialogue;
        GameManager.instance.OnDialogueStart?.Invoke();
        dialogo.Iniciar();

        gameObject.SetActive(true);
        ExibirFalaAtual();  
    }

    public void Avancar()
    {
        GameManager.instance.OnEndDialogueSentence?.Invoke();
        if (dialogo.TemProximaFala())
        {
            dialogo.Avancar();
            ExibirFalaAtual();
        }
        else 
        {
            Esconder();
        }

    }

    private void Esconder () 
    {
        isSpeaking = false;
        gameObject.SetActive(false);
        GameManager.instance.OnDialogueEnd?.Invoke();
        Personagens.instance.Interact();
    }

    private void ExibirFalaAtual()
    {
        FalaDialogo falaAtual = dialogo.FalaAtual;
        PersonagemDialogo personagem = falaAtual.PersonagemDialogo;

        textoNomePersonagem.text = personagem.Nome;
        //this.textoFalaDialogo.text = falaAtual.Texto;

        if (this.preencherTextoCoroutine != null)
        {
            StopCoroutine(this.preencherTextoCoroutine);
        }
        this.preencherTextoCoroutine = StartCoroutine(PreencherConteudoTextoAosPoucos(falaAtual.Texto));
    }

    private IEnumerator PreencherConteudoTextoAosPoucos(string texto)
    {
        textoFalaDialogo.text = "";
        for (int i = 0; i < texto.Length; i++)
        {
            textoFalaDialogo.text += texto[i];
            AudioManager.instance.PlayAudio(characterVoice);
            yield return new WaitForSeconds(this.intervaloTempoEntreLetrasEmSegundos);
        }
    }
}
