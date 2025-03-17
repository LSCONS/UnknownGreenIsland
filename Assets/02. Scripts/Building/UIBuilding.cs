using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : MonoBehaviour
{
    public BuildingManager buildingManager;
    public SelectedBuildingType selectedBuildingType;
    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 mousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;
    
    public int curBuildItem;
    public int buildItem;
    private int oldBuildItem;


    private void Awake()
    {
        buildingManager = GetComponentInParent<BuildingManager>();
    }

    private void Start()
    {
        buildItem = buttons.Count;
        foreach (MenuButton button in buttons)
        {
            button.sceneImage.color = button.normalcolor;
        }
        curBuildItem = 0;
        oldBuildItem = 0;
    }
    
    public void GetCurrentBuild()
    {
        mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        toVector2M = new Vector2 (mousePosition.x/Screen.width, mousePosition.y/Screen.height);
        float angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360;
        }

        curBuildItem = (int)(angle / (360 / buildItem));

        if (curBuildItem != oldBuildItem)
        {
            buttons[oldBuildItem].sceneImage.color = buttons[oldBuildItem].normalcolor;
            oldBuildItem = curBuildItem;
            buttons[curBuildItem].sceneImage.color = buttons[curBuildItem].HighlightColor;
        }
    }

    public void ButtonAction()
    {
        buttons[curBuildItem].sceneImage.color = buttons[curBuildItem].PressedColor;
        if (curBuildItem == 0)
        {
            buildingManager.currentBuildType = SelectedBuildingType.floor;
            buildingManager.currentBuildingIndex = 0;
        }
        if (curBuildItem == 1)
        {
            buildingManager.currentBuildType = SelectedBuildingType.wall;
            buildingManager.currentBuildingIndex = 1;
        }
        if (curBuildItem == 2)
        {
            buildingManager.currentBuildType = SelectedBuildingType.wall;
            buildingManager.currentBuildingIndex = 2;
        }
        if (curBuildItem == 3)
        {
            buildingManager.currentBuildType = SelectedBuildingType.wall;
            buildingManager.currentBuildingIndex = 0;
        }
        if (curBuildItem == 4)
        {
            buildingManager.currentBuildType = SelectedBuildingType.wall;
            buildingManager.currentBuildingIndex = 3;
        }
    }
}

[System.Serializable]
public class MenuButton
{
    public string name;
    public Image sceneImage;
    public Color normalcolor = Color.white;
    public Color HighlightColor = Color.grey;
    public Color PressedColor = Color.grey;
}
