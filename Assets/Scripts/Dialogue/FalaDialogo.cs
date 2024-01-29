using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FalaDialogo
{

    [SerializeField, HideInInspector]private string identificador;

    [SerializeField] private PersonagemDialogo personagemDialogo;

    [SerializeField, TextArea(minLines: 3, maxLines: 10)] private string texto;


    public PersonagemDialogo PersonagemDialogo
    {
        get
        {
            return this.personagemDialogo;
        }
    }
    public string Texto
    {
        get 
        {
            return this.texto;
        }
    }

    public void AtualizarIdentificador()
    {
        if ((this.personagemDialogo != null) && (this.texto != null))
        {
            this.identificador = "[" + this.personagemDialogo.Nome + "]" + this.texto;
        }
    }

}
