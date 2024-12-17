using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Image self;
    public PlayerMovement player;
    private Vector2 originalSize;

    private void Awake()
    {
        self = GetComponent<Image>();
        originalSize = self.rectTransform.sizeDelta;
    }

    public void ChangeSize()
    {
        float multiplier = (player.Health / player.startingHealth);
        //Debug.Log("Multiplier: " + multiplier);
        //Debug.Log("ui health change called");
        self.rectTransform.sizeDelta = new Vector2(200*multiplier, originalSize.y);
    }
}
