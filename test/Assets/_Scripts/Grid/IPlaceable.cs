
using UnityEngine;

public interface IPlaceable
{
    public bool IsPlaced{get;set;}
    public int Index{get;set;}
    public GameObject GetGameObject();
    public Vector2Int GetSize{ get; set;}
    public Vector2Int GetRange{get;set;}
    public Vector3Int PlacedPosition{get;set;}
    public GameObject Orientation{get;set;}
    public void Placed();
    public void UnPlaced();
    public void OnDestroyObject();
    
}
