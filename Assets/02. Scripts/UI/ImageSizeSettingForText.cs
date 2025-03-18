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

    public AbnormalButtonHandler BleedingHanler;
    public AbnormalButtonHandler PosioningImage;
    public AbnormalButtonHandler SpeedUpImage;
    public AbnormalButtonHandler SpeedDownImage;
    public AbnormalButtonHandler FactureImage;
    public AbnormalButtonHandler HealthUpImage;
    public AbnormalButtonHandler HealthDownImage;
    public AbnormalButtonHandler PowerUpImage;
    public AbnormalButtonHandler PowerDownImage;
    public AbnormalButtonHandler StaminaUpImage;
    public AbnormalButtonHandler StaminaDownImage;

    public AbnormalButtonHandler[] abnormalButtonHandlers;


    private void OnValidate()
    {
        childText = transform.GetComponentForTransformFindName<TextMeshProUGUI>("AbnormalText");
        parentImage = transform.GetComponentForTransformFindName<Image>("TextImage");

        textRect = childText.GetComponent<RectTransform>();
        parentRect = parentImage.GetComponent<RectTransform>();

        BleedingHanler = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("BleedingImage");
        PosioningImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("PosioningImage");
        SpeedUpImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("SpeedUpImage");
        SpeedDownImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("SpeedDownImage");
        FactureImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("FactureImage");
        HealthUpImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("HealthUpImage");
        HealthDownImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("HealthDownImage");
        PowerUpImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("PowerUpImage");
        PowerDownImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("PowerDownImage");
        StaminaUpImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("StaminaUpImage");
        StaminaDownImage = transform.GetComponentForTransformFindName<AbnormalButtonHandler>("StaminaDownImage");

        BleedingHanler.abnormalStatus = AbnormalStatus.Bleeding;
        PosioningImage.abnormalStatus = AbnormalStatus.Poisoning;
        SpeedUpImage.abnormalStatus = AbnormalStatus.PlentyWater;
        SpeedDownImage.abnormalStatus = AbnormalStatus.Dehydrration;
        FactureImage.abnormalStatus = AbnormalStatus.Fracture;
        HealthUpImage.abnormalStatus = AbnormalStatus.Eat;
        HealthDownImage.abnormalStatus = AbnormalStatus.Hunger;
        PowerUpImage.abnormalStatus = AbnormalStatus.EatFull;
        PowerDownImage.abnormalStatus = AbnormalStatus.Starvation;
        StaminaUpImage.abnormalStatus = AbnormalStatus.Drink;
        StaminaDownImage.abnormalStatus = AbnormalStatus.Thirsty;

        abnormalButtonHandlers = new AbnormalButtonHandler[]
        {
        BleedingHanler,
        PosioningImage,
        SpeedUpImage,
        SpeedDownImage,
        FactureImage,
        HealthUpImage,
        HealthDownImage,
        PowerUpImage,
        PowerDownImage,
        StaminaUpImage,
        StaminaDownImage
        };
    }

    private void OnEnable()
    {
        parentImage.gameObject.SetActive(false);
    }

    public void UpdateAbnormalUI(Dictionary<AbnormalStatus, int> dict)
    {

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
