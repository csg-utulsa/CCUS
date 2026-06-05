using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlowFillMaskSpriteSwap : MonoBehaviour
{
    public Image maskRenderer;
    public Sprite[] spriteMaskImages;
    public float timeBetweenSpriteSwaps = 0.067f;
    private int currentSprite = 0;
    private float timer = 0f;
    public bool isAnimating {get; set;} = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maskRenderer = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimating){
            timer += Time.deltaTime;
            if(timer > timeBetweenSpriteSwaps){
                timer = 0f;
                currentSprite++;
                if(currentSprite >= spriteMaskImages.Length){
                    currentSprite = 0;
                }
                maskRenderer.sprite = spriteMaskImages[currentSprite];
            }
        }
    }
}
