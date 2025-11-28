using System.Collections.Generic;
using UnityEngine;

public interface IElectricNode 
{
    public List<IElectricNode> Neighbours{get;set;}
    public bool Power{get;set;}
    public Transform ConnectSocket{get;set;}
    public bool CanTie(IElectricNode target);
    public void PowerChanged(bool powerState);
}
