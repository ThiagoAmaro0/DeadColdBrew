using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] Sprite[] cardSprite;
    [SerializeField] Image cardImage;
    SpriteRenderer cardSpr;
    public void Show()
    {
        cardSpr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        switch (GameManager.instance.cardSystem.GetCard())
        {
            case CardSystem.Type.Tower:
                cardSpr.sprite = cardSprite[0];
                cardImage.sprite = cardSprite[0];
                break;
            case CardSystem.Type.Sun:
                cardSpr.sprite = cardSprite[1];
                cardImage.sprite = cardSprite[1];
                break;
            case CardSystem.Type.WOF:
                cardSpr.sprite = cardSprite[2];
                cardImage.sprite = cardSprite[2];
                break;
            default:
                break;
        };
    }
    public void End()
    {
        cardImage.enabled = GameManager.instance.cardSystem.GetCard()!=CardSystem.Type.None;
        btn.interactable = true;
    }
    
}
