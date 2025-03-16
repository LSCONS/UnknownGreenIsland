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
    /// ��ȣ�ۿ�� �������� �����ܰ� ������ �ʱ�ȭ�ϴ� �޼���
    /// </summary>
    /// <param name="itemData">����� ������ ��� ItemData</param>
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
