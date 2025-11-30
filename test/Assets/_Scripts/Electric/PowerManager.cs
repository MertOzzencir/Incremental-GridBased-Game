using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    void Start()
    {
        ElectricPipelineSelecter.OnPipelineChanged += CheckElectricLines;
    }

    private void CheckElectricLines(IElectricNode target)
    {
        Queue<IElectricNode> queue = new Queue<IElectricNode>();
        HashSet<IElectricNode> visited = new HashSet<IElectricNode>();
        queue.Enqueue(target);
        visited.Add(target);
        while(queue.Count != 0)
        {
            IElectricNode current = queue.Dequeue();
            foreach(var a in current.Neighbours)
            {
                if (!visited.Contains(a))
                {
                    
                    visited.Add(a);
                    queue.Enqueue(a);
                }
            }
        }
        bool hasPower = false;
        foreach(var a in visited)
        {
            if(a is IGenerator)
            {
                hasPower = true;
                break;
            }
        }

        if(!hasPower)
            return;

        foreach(var a in visited)
        {
            a.PowerChanged(true);
        }
      
    }
}
