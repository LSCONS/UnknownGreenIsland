using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    [Header("Build Objects")]
    [SerializeField] private List<GameObject> floorObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> wallObjects = new List<GameObject>();

    [Header("Build Settings")]
    public SelectedBuildingType currentBuildType;
    [SerializeField] private LayerMask connectorLayer;
    public GameObject buildingUI;

    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
    [SerializeField] private float connectorOverlapRadius = 2f;
    [SerializeField] private float maxGroundAngle = 45f;

    [Header("Internal State")]
    public bool isBuilding = false;
    public int currentBuildingIndex;
    private GameObject ghostBuildGameobject;
    private bool isGhostInValidPosition = false;
    private Transform ModelParent = null;
    private UIBuilding uiBuilding;

    private void Awake()
    {
        uiBuilding = GetComponentInChildren<UIBuilding>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            isBuilding = !isBuilding;

        if (isBuilding)
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                isBuilding = false;
                buildingUI.SetActive(true);
                Util.CursorisLock(false);
            }
        }
        else if (buildingUI.activeSelf) 
        {
            if (Input.GetMouseButtonUp(1)) 
            {
                isBuilding = true;
                buildingUI.SetActive(false);
                Util.CursorisLock(true);
            }
        }

        if (buildingUI.activeSelf)
        {
            uiBuilding.GetCurrentBuild();
            if (Input.GetMouseButtonDown(0))
            {
                uiBuilding.ButtonAction();
            }
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            RemoveBuild();
        }

        if (isBuilding)
        {
            GhostBuild();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentBuildType == SelectedBuildingType.wall && currentBuildingIndex == 3)
                {
                    ghostBuildGameobject.gameObject.transform.Rotate(90, 0, 0);
                }
            }

            if (Input.GetMouseButtonDown(0))
                PlaceBuild();
        }
        else if (ghostBuildGameobject)
        {
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;
        }
    }

    private void GhostBuild()
    {
        GameObject currentBuild = GetCurrentBuild();
        CreateGhostPrefab(currentBuild);

        MoveGhostPrefabToRaycast();
        CheckBuildValidity();
    }

    private void CreateGhostPrefab(GameObject currentBuild)
    {
        if (ghostBuildGameobject == null)
        {
            ghostBuildGameobject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameobject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameobject.transform);
        }
        else if (ghostBuildGameobject.name != currentBuild.name + "(Clone)")
        {
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = Instantiate(currentBuild);

            ModelParent = ghostBuildGameobject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameobject.transform);
        }
    }

    private void MoveGhostPrefabToRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            ghostBuildGameobject.transform.position = hit.point;
        }
    }

    private void CheckBuildValidity()
    {
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameobject.transform.position, connectorOverlapRadius, connectorLayer);
        if (colliders.Length > 0)
        {
            GhostConnectBuild(colliders);
        }
        else
        {
            GhostSeparateBuild();
        }
    }

    private void GhostConnectBuild(Collider[] colliders)
    {
        Connector bestConnector = null;

        foreach (Collider collider in colliders)
        {
            Connector connector = collider.GetComponent<Connector>();

            if (connector.canConnectTo)
            {
                bestConnector = connector;
                break;
            }
        }

        if (bestConnector == null || currentBuildType == SelectedBuildingType.floor && bestConnector.isConnectedToFloor || currentBuildType == SelectedBuildingType.wall && bestConnector.isConnectedToWall)
        {
            GhostifyModel(ModelParent, ghostMaterialInvalid);
            isGhostInValidPosition = false;
            return;
        }

        SnapGhostPrefabToConnector(bestConnector);
    }

    private void SnapGhostPrefabToConnector(Connector connector)
    {
        Transform ghostConnector = FindSnapConnector(connector.transform, ghostBuildGameobject.transform.GetChild(1));
        ghostBuildGameobject.transform.position = connector.transform.position - (ghostConnector.position - ghostBuildGameobject.transform.position);

        if (currentBuildType == SelectedBuildingType.wall)
        {
            Quaternion newRotation = ghostBuildGameobject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameobject.transform.rotation = newRotation;
        }

        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }

    private void RemoveBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject target = hit.collider.gameObject;
            Transform parentTransform = target.transform.root;
            GameObject objectToDestroy = parentTransform.gameObject;
            LayerMask targetLayer = LayerMask.NameToLayer("BuildingPre");

            if (target.layer == targetLayer)
            {
                float searchRadius = 5f;

                
                Collider[] nearbyColliders = Physics.OverlapSphere(objectToDestroy.transform.position, searchRadius, connectorLayer);
                List<Connector> nearbyConnectors = new List<Connector>();

                foreach (Collider collider in nearbyColliders)
                {
                    Connector nearbyConnector = collider.GetComponent<Connector>();
                    if (nearbyConnector != null)
                    {
                        nearbyConnectors.Add(nearbyConnector);
                    }
                }

               
                Destroy(objectToDestroy);

                
                StartCoroutine(UpdateConnectorsNextFrame(nearbyConnectors));
            }
        }
    }

    private IEnumerator UpdateConnectorsNextFrame(List<Connector> nearbyConnectors)
    {
        yield return null; 

        foreach (Connector connector in nearbyConnectors)
        {
            connector.UpdateConnectors(true);
        }
    }

    private void GhostSeparateBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (currentBuildType == SelectedBuildingType.wall)
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (hit.collider.transform.root.CompareTag("Buildables"))
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle)
            {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition = true;
            }
            else
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private Transform FindSnapConnector(Transform snapConnector, Transform ghostConnectorParent)
    {
        ConnectorPosition OppositeConnectorTag = GetOppositePosition(snapConnector.GetComponent<Connector>());

        foreach (Connector connector in ghostConnectorParent.GetComponentsInChildren<Connector>())
        {
            if (connector.connectorPosition == OppositeConnectorTag)
                return connector.transform;
        }
        return null;
    }

    private ConnectorPosition GetOppositePosition(Connector connector)
    {
        ConnectorPosition position = connector.connectorPosition;

        if (currentBuildType == SelectedBuildingType.wall && connector.connectorParentType == SelectedBuildingType.floor)
            return ConnectorPosition.bottom;

        if (currentBuildType == SelectedBuildingType.floor && connector.connectorParentType == SelectedBuildingType.wall && connector.connectorPosition == ConnectorPosition.top)
        {
            if (connector.transform.root.rotation.y == 0)
            {
                return GetConnectorClosestToPlayer(true);
            }
            else
            {
                return GetConnectorClosestToPlayer(false);
            }
        }

        switch (position)
        {
            case ConnectorPosition.left:
                return ConnectorPosition.right;
            case ConnectorPosition.right:
                return ConnectorPosition.left;
            case ConnectorPosition.bottom:
                return ConnectorPosition.top;
            case ConnectorPosition.top:
                return ConnectorPosition.bottom;
            default:
                return ConnectorPosition.bottom;
        }
    }

    private ConnectorPosition GetConnectorClosestToPlayer(bool topBottom)
    {
        Transform cameraTransform = Camera.main.transform;

        if (topBottom)
            return cameraTransform.position.z >= ghostBuildGameobject.transform.position.z ? ConnectorPosition.bottom : ConnectorPosition.top;
        else
            return cameraTransform.position.x >= ghostBuildGameobject.transform.position.x ? ConnectorPosition.left : ConnectorPosition.right;
    }

    private void GhostifyModel(Transform modelParent, Material ghostMaterial = null)
    {
        if (ghostMaterial != null)
        {
            foreach (MeshRenderer meshRenderer in modelParent.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = ghostMaterial;
            }
        }
        else
        {
            foreach (Collider modelColliders in modelParent.GetComponentsInChildren<Collider>())
            {
                modelColliders.enabled = false;
            }
        }
    }

    private GameObject GetCurrentBuild()
    {
        switch (currentBuildType)
        {
            case SelectedBuildingType.floor:
                return floorObjects[currentBuildingIndex];
            case SelectedBuildingType.wall:
                return wallObjects[currentBuildingIndex];
        }
        return null;
    }

    private void PlaceBuild()
    {
        if (ghostBuildGameobject != null & isGhostInValidPosition)
        {
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameobject.transform.position, ghostBuildGameobject.transform.rotation);

            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;

            foreach (Connector connector in newBuild.GetComponentsInChildren<Connector>())
            {
                connector.UpdateConnectors(true);
            }
        }
    }
}

[System.Serializable]

public enum SelectedBuildingType
{
    floor,
    wall
}
