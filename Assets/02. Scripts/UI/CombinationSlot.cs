using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombinationSlot : MonoBehaviour
{

    public Image image;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI description;
    public TextMeshProUGUI recipeText;
    public ItemObject itemObject;
    public Button button;

    private void OnValidate()
    {
        image = transform.GetComponentForTransformFindName<Image>("Icon");
        titleText = transform.GetComponentForTransformFindName<TextMeshProUGUI>("ItemName");
        description = transform.GetComponentForTransformFindName<TextMeshProUGUI>("ItemDescription");
        recipeText = transform.GetComponentForTransformFindName<TextMeshProUGUI>("Recipe");
        button = transform.GetChildComponentDebug<Button>();
        button.onClick.AddListener(TryCreateItem);
    }

    public void UpdateData(ItemObject itemObject)
    {
        ItemData data = itemObject.data;

        image.sprite = data.inventory_icon;
        titleText.text = data.ItemName;
        description.text = data.description;
        StringBuilder recipeTextStringBuilder = new StringBuilder();
        for(int i = 0; i < data.resources.Length; i++)
        {
            recipeTextStringBuilder.Append(data.resources[i].type.ToString() + data.resources[i].Amount.ToString() + "개/");
        }
        recipeText.text = recipeTextStringBuilder.ToString();
    }


    //아이템 생성을 시도하는 메서드
    private void TryCreateItem()
    {
        if (transform.GetComponentInparentDebug<PlayerInventoty>().CreateItem(itemObject))
        {
            Debug.Log("생성 성공");
        }
        else
        {
            Debug.Log("생성 실패");
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
