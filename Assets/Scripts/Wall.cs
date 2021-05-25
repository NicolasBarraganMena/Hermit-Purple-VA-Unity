using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    [Header("Control Muro")]
    public Sprite dmgSprite;
    public int hp = 4;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Daño(int daño)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= daño;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
