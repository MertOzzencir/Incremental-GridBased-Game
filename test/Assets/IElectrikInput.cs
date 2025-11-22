
using System.Collections.Generic;
using UnityEngine;

public interface IElectrikInput
{
    public float PowerConsumePerSecond{get;set;}
    public void GetCharge(float electricInput);
    public List<IElectrikOutput> ElectrikSources{get;set;}
    public int OutputConnectLimit{get;set;}
    public void ConnectWithOutput(IElectrikOutput source);
    public Transform InputSocket{get;set;}
    public GameObject GetGameObject();
    public bool IsAvaliable(IElectrikOutput selectedOutput);

}
