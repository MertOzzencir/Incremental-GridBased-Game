using UnityEngine;

public interface IElectrikOutput
{
    public GameObject GetGameObject();
    public Transform OutputSocket{get;set;}
}
