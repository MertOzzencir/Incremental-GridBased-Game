using System;
using System.Collections;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] private GameObject gridIndicatorPrefab;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private int gridSize;
    [SerializeField] private float rotationSpeed;

    private IPlaceable placeObject;
    private IPlaceable lastPlaceObjectPrefab;
    private Vector3Int choosedPoint;
    private GameObject indicator;
    private RotationState rotationState;
    private Vector2Int assignedSize;
    private Vector2Int assignedRange;
    private Coroutine targetPositionCO;

    void Start()
    {
        rotationState = 0;
        InputManager.Instance.OnLeftClick += PlaceObject;
        InputManager.Instance.OnRotate += RotateObjectByButton;
        InputManager.Instance.OnBuildMode += BuildModeChecker;
        indicator = Instantiate(gridIndicatorPrefab);

    }

    private void BuildModeChecker(bool obj)
    {
        if (!obj)
        {
            if (lastPlaceObjectPrefab != null)
                UnselectObject();
        }
    }

    private void Update()
    {
        if (placeObject == null)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayerMask))
        {
            choosedPoint = new Vector3Int(Mathf.FloorToInt(hit.point.x / gridSize) * gridSize,
                                                     0,
                                                     Mathf.FloorToInt(hit.point.z / gridSize) * gridSize);
            indicator.transform.position = choosedPoint;
            indicator.transform.localScale = new Vector3(placeObject.GetSize.x, indicator.transform.localScale.y, placeObject.GetSize.y);
            placeObject.GetGameObject().transform.position = choosedPoint;
        }


    }
    private void PlaceObject()
    {
        if (placeObject == null)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayerMask))
        {

            GridSystem.Instance.CheckPoint(choosedPoint, new GridSpec(placeObject), out bool isValid);
            if (isValid)
            {
                placeObject.Placed();
                placeObject = null;
            }
        }
    }
    public void SetObject(IPlaceable recievedObject)
    {
        GameObject refGO = Instantiate(recievedObject.GetGameObject());
        placeObject = refGO.GetComponent<IPlaceable>();
        placeObject.GetGameObject().GetComponent<IPickable>().Picked();
        assignedSize = placeObject.GetSize;
        assignedRange = placeObject.GetRange;
        InitiazeRotation();
    }
    public void RecieveObject(IPlaceable recievedObject)
    {
        if (targetPositionCO != null)
            return;
        lastPlaceObjectPrefab = recievedObject;
        indicator.SetActive(true);
        if (placeObject != null)
        {
            if (placeObject.Index == recievedObject.Index)
            {
                UnselectObject();
                return;
            }
            else
            {
                Destroy(placeObject.GetGameObject());
                SetObject(recievedObject);
                return;
            }
        }
        else
            SetObject(recievedObject);

    }

    private void UnselectObject()
    {
        if (placeObject != null)
        {
            placeObject.UnPlaced();
            placeObject = null;
            indicator.SetActive(false);
        }
    }

    private void RotateObjectByButton()
    {
        if (placeObject == null || targetPositionCO != null)
            return;
        rotationState = (RotationState)(((int)rotationState + 1) & 3);
        RootRotation(out Quaternion target, out Vector3Int targetPosition);
        targetPositionCO = StartCoroutine(TargetPositionHandler(target, targetPosition));
    }
    private void InitiazeRotation()
    {
        RootRotation(out Quaternion target, out Vector3Int targetPosition);
        placeObject.Orientation.transform.rotation = target;
        placeObject.Orientation.transform.localPosition = targetPosition;
    }

    private void RootRotation(out Quaternion target, out Vector3Int targetPosition)
    {
        Vector3 targetRotation = Vector3.zero;
        target = Quaternion.Euler(0, 0, 0);
        targetPosition = Vector3Int.zero;
        switch (rotationState)
        {
            case RotationState.Kuzey:
                targetRotation = new Vector3(0, 0, 0);
                targetPosition = Vector3Int.zero;
                placeObject.GetSize = assignedSize;
                placeObject.GetRange = assignedRange;
                break;

            case RotationState.Bati:
                targetRotation = new Vector3(0, -90, 0);
                targetPosition = new Vector3Int(assignedSize.y, 0, 0);
                placeObject.GetSize = new Vector2Int(assignedSize.y, assignedSize.x);
                placeObject.GetRange = new Vector2Int(assignedRange.y, assignedRange.x);
                break;

            case RotationState.Güney:
                targetRotation = new Vector3(0, -180, 0);
                targetPosition = new Vector3Int(assignedSize.x, 0, assignedSize.y);
                placeObject.GetSize = assignedSize;
                placeObject.GetRange = assignedRange;
                break;

            case RotationState.Doğu:
                targetRotation = new Vector3(0, -270, 0);
                targetPosition = new Vector3Int(0, 0, assignedSize.x);
                placeObject.GetSize = new Vector2Int(assignedSize.y, assignedSize.x);
                placeObject.GetRange = new Vector2Int(assignedRange.y, assignedRange.x);
                break;
        }
        target = Quaternion.Euler(targetRotation);
    }

    IEnumerator TargetPositionHandler(Quaternion targetRotation, Vector3Int targetPosition)
    {
        var placedSpec = placeObject.Orientation.transform;
        while (Quaternion.Angle(placedSpec.transform.rotation, targetRotation) > 0.1f || Vector3.Distance(placedSpec.localPosition, targetPosition) > 0.1f)
        {
            placedSpec.rotation = Quaternion.Lerp(placedSpec.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            placedSpec.localPosition = Vector3.Lerp(placedSpec.localPosition, targetPosition, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        placeObject.Orientation.transform.rotation = targetRotation;
        placeObject.Orientation.transform.localPosition = targetPosition;

        targetPositionCO = null;
    }
}

public enum RotationState
{

    Kuzey = 0,
    Bati = 1,
    Güney = 2,
    Doğu = 3
}
