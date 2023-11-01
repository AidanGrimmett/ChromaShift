using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlashAnimation : MonoBehaviour
{
    [SerializeField] private GameObject flashImage;
    private Image image;
    private Color oldColor;

    public void Awake()
    {
        image = flashImage.GetComponent<Image>();
        oldColor = ColorController.currentColor;
    }

    private void Update()
    {
        if (oldColor != ColorController.currentColor)
        {
            FlashColor();
            oldColor = ColorController.currentColor;
        }
    }

    public void FlashDeath()
    {
        image.color = Color.white;

        flashImage.SetActive(true);
    }

    public void FlashColor()
    {
        image.color = ColorController.currentColor;

        flashImage.SetActive(true);
    }
}
