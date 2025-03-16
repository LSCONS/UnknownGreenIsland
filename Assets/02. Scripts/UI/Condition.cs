using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    private Image barImage;

    private void OnValidate()
    {
        barImage = transform.GetComponentForTransformFindName<Image>("Bar");
    }

    public void UpdateBar(float value)
    {
        barImage.fillAmount = value;
    }
}
