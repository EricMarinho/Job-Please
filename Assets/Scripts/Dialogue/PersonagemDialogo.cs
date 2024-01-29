using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Novo Ator", menuName = "Sistema de diálogo/Novo ator")]
public class PersonagemDialogo : ScriptableObject
{

    [SerializeField]private string nome;

    public string Nome
    {
        get 
        {
            return this.nome;
        }
    }
}
