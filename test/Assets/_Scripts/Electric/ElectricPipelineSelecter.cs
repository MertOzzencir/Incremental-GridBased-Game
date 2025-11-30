using System;
using UnityEngine;

public class ElectricPipelineSelecter : MonoBehaviour
{
    [SerializeField] private GameObject ropePrefab;
    public static event Action<IElectricNode> OnPipelineChanged;
    private IElectricNode selectedNode;

    void Start()
    {
        InputManager.Instance.OnMouseHoverAction += PipeSelection;
    }

    private void PipeSelection(bool obj)
    {
        if (obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, Mathf.Infinity);
            foreach (var a in hits)
            {
                if (a.transform.TryGetComponent(out selectedNode))
                {
                    Debug.Log("Found the object");
                    return;
                }
            }
        }
        else
        {
            if (selectedNode == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, Mathf.Infinity);
            foreach (var a in hits)
            {
                if (a.transform.TryGetComponent(out IElectricNode target) && selectedNode != target)
                {
                    if (target.CanTie(selectedNode))
                    {

                        Debug.Log("Connected");
                        RopeConnection(target);
                        selectedNode.Neighbours.Add(target);
                        target.Neighbours.Add(selectedNode);
                        OnPipelineChanged?.Invoke(target);
                        // foreach (var a in selectedNode.Neighbours)
                        //     Debug.Log("Selected neighbours: " + a);
                        // foreach (var a in target.Neighbours)
                        //     Debug.Log("Target neighbours: " + a);
                    }
                }
            }

        }
    }

    private void RopeConnection(IElectricNode target)
    {
        GameObject rope = Instantiate(ropePrefab);
        rope.transform.position = selectedNode.ConnectSocket.position;
        Vector3 directionVector = (target.ConnectSocket.position - selectedNode.ConnectSocket.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionVector);
        rope.transform.rotation = lookRotation;
        float distance = Vector3.Distance(target.ConnectSocket.position, selectedNode.ConnectSocket.position);
        rope.transform.localScale = new Vector3(rope.transform.localScale.x, rope.transform.localScale.y, distance);
        rope.transform.parent = target.ConnectSocket;
    }
}
