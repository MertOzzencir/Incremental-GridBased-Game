
using UnityEngine;

public interface IPlaceable
{

    public int Index{get;set;}
    public GameObject GetGameObject();
    public Vector2Int GetSize{ get; set;}
    public Vector3Int PlacedPosition{get;set;}
    public GameObject Orientation{get;set;}
    public void Placed();
    public void OnDestroyObject();
}
