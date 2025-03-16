using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInteraction : MonoBehaviour
{
    private TextMeshProUGUI interactionText;
    private Image image;

    private void OnValidate()
    {
        interactionText = transform.GetComponentForTransformFindName<TextMeshProUGUI>("Text");
        image = transform.GetComponentForTransformFindName<Image>("Icon");
    }


    /// <summary>
    /// 상호작용시 아이템의 아이콘과 정보를 초기화하는 메서드
    /// </summary>
    /// <param name="itemData">출력할 정보가 담긴 ItemData</param>
    public void UpdateData(ItemData itemData)
    {
        if(itemData != null)
        {
            interactionText.text = itemData.interaction_information;
            image.sprite = itemData.icon;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
