using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuilding : MonoBehaviour
{
    public BuildingManager buildingManager;
    public SelectedBuildingType selectedBuildingType;

    private void Awake()
    {
        buildingManager = GetComponentInParent<BuildingManager>();
    }

    public void ChangetoFloor()
    {
        buildingManager.currentBuildType = SelectedBuildingType.floor;
        buildingManager.currentBuildingIndex = 0;
    }

    public void ChangetoWall()
    {

    }

    public void ChangetoHalfWall()
    {

    }

    public void ChangetoDoorWall()
    {

    }

    public void ChangetoRoofWall()
    {

    }
}
