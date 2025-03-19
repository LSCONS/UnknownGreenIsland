using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbnormalButtonHandler : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    public ImageSizeSettingForText imageSizeSetting;
    public RectTransform rect;
    public string text;
    public Image fillAmount1;
    public Image fillAmount2;

    public AbnormalStatus abnormalStatus;

    private void OnValidate()
    {
        imageSizeSetting = transform.GetComponentInparentDebug<ImageSizeSettingForText>();
        rect = transform.parent.GetChildComponentDebug<RectTransform>();
        fillAmount1 = transform.parent.GetComponentForTransformFindName<Image>("FillAmount1");
        fillAmount2 = transform.parent.GetComponentForTransformFindName<Image>("FillAmount2");
    }

    public void ChangeFillAmount(float value)
    {
        fillAmount1.fillAmount = value / 10;
        fillAmount2.fillAmount = value / 10;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imageSizeSetting.ExitMousePoint();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        imageSizeSetting.SettingSizeImage(rect, text);
    }
}
