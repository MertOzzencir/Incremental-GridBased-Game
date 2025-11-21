using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance;
    public event Action<List<Vector3Int>> OnDeletedGrid;

    private Dictionary<Vector3Int, GridSpec> grid = new Dictionary<Vector3Int, GridSpec>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void CheckPoint(Vector3Int pivotPoint, GridSpec objectToPlace, out bool feedbackValid)
    {
        bool isValid = true;
        feedbackValid = false;
        List<Vector3Int> pointsToAssign = new List<Vector3Int>();
        for (int i = 0; i < objectToPlace.ObjectSize.y; i++)
        {
            for (int j = 0; j < objectToPlace.ObjectSize.x; j++)
            {
                Vector3Int point = new Vector3Int(pivotPoint.x + j, 0, pivotPoint.z + i);
                if (!grid.ContainsKey(point))
                {
                    pointsToAssign.Add(point);
                }
                else
                    isValid = false;
            }
        }
        if (isValid)
        {
            feedbackValid = isValid;
            AddOnGrid(pivotPoint, objectToPlace, pointsToAssign);
            //Debug.Log("Valid?");    
        }
    }

    private void AddOnGrid(Vector3Int pivotPoint, GridSpec value, List<Vector3Int> pointToAssign)
    {

        foreach (var a in pointToAssign)
        {
            grid.Add(a, value);
        }
        value.PlacedObject.GetGameObject().transform.position = pivotPoint;
        value.PlacedObject.PlacedPosition = pivotPoint;
    }
    public void DeleteOnGrid(Vector3Int placedPosition, IPlaceable placedObject)
    {
        List<Vector3Int> deletedPoints = new List<Vector3Int>();
        for (int i = 0; i < placedObject.GetSize.y; i++)
        {
            for (int j = 0; j < placedObject.GetSize.x; j++)
            {
                Vector3Int point = new Vector3Int(placedPosition.x + j, 0, placedPosition.z + i);
                if (grid.ContainsKey(point))
                {
                    deletedPoints.Add(point);
                    grid.Remove(point);
                }
            }
        }
        Debug.Log("sa?");
        OnDeletedGrid?.Invoke(deletedPoints);
    }

    [ContextMenu("Grid Dict")]

    public void GridShow()
    {
        foreach (var a in grid)
        {
            Debug.Log($"{a.Key} Position has {a.Value} GridSpec | {a.Value.PlacedObject} and size is {a.Value.ObjectSize} ");
        }
    }
}


public struct GridSpec
{
    public IPlaceable PlacedObject;
    public Vector2Int ObjectSize;
    public GridSpec(IPlaceable _placedObject)
    {
        PlacedObject = _placedObject;
        ObjectSize = _placedObject.GetSize;

    }


}
