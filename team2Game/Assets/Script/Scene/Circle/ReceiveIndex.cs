using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveIndex : MonoBehaviour
{
    [SerializeField]
    RouletteImage roulette;

    [SerializeField]
    private Sprite[] sprites;

    private Image spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        roulette.GetComponent<RouletteImage>();
        spriteRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveImage();
    }

    void ReceiveImage()
    {
        spriteRenderer.sprite = sprites[roulette.CurrentNum()-1];
    }
}
