using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Novo Dialogo", menuName = "Sistema de diálogo/Novo diálogo")]
public class Dialogo : ScriptableObject
{
    [SerializeField]private FalaDialogo[] falasDialogo;

    private int indiceFalaAtual;

    private void OnValidate()
    {
        foreach (FalaDialogo falaDialogo in falasDialogo) 
        {
            falaDialogo.AtualizarIdentificador();
        }
    }

    public void Iniciar()
    {
        this.indiceFalaAtual = 0;
    }

    public FalaDialogo FalaAtual
    {
        get
        {
            if (this.indiceFalaAtual < this.falasDialogo.Length)
            {
                return this.falasDialogo[this.indiceFalaAtual];
            }

            return null;
        }
    }

    public void Avancar()
    {
        if (TemProximaFala())
        {
            this.indiceFalaAtual++;
        }
    }

    public bool TemProximaFala()
    {
        if (this.indiceFalaAtual < (this.falasDialogo.Length - 1))
        {
            return true;
        }
        return false;
    }
}