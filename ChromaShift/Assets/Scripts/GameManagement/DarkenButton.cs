using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color normalColor;
    private Color hoverColor;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        normalColor = Color.white;
        hoverColor = new Color(1,1,1,0.75f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = normalColor;
    }
}