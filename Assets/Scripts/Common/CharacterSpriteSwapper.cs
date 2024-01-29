using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteSwapper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.instance.OnInterviewStart += SwapStartingSprite;
    }

    public void SwapStartingSprite ()
    {
        spriteRenderer.sprite = GameManager.instance.GetCurrentInterview().normalCharacterSprite;
    }
}
