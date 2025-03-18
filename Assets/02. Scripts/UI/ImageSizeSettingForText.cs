using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSizeSettingForText : MonoBehaviour
{
    public TextMeshProUGUI childText;
    public Image parentImage;

    RectTransform textRect;
    RectTransform parentRect;


    private void OnValidate()
    {
        childText = transform.GetComponentForTransformFindName<TextMeshProUGUI>("AbnormalText");
        parentImage = transform.GetComponentForTransformFindName<Image>("TextImage");

        textRect = childText.GetComponent<RectTransform>();
        parentRect = parentImage.GetComponent<RectTransform>();
    }


    public void SettingSizeImage(RectTransform position, string text)
    {
        childText.text = text;

        parentImage.gameObject.SetActive(true);

        parentRect.sizeDelta = textRect.sizeDelta + new Vector2(30, 30);

        parentImage.rectTransform.anchoredPosition = position.anchoredPosition + new Vector2(50, 100);
    }


    public void ExitMousePoint()
    {
        parentImage.gameObject.SetActive(false);
    }
}
