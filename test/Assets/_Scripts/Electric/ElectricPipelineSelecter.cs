using System;
using System.Collections;
using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

public class ElectricPipelineSelecter : MonoBehaviour
{
    [SerializeField] private Rope rope;
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

                        Rope ropesa = Instantiate(rope);
                        ropesa.transform.position = selectedNode.ConnectSocket.position;
                        ropesa.transform.parent = selectedNode.ConnectSocket.transform;
                        ropesa.startPoint = target.ConnectSocket;
                        ropesa.endPoint = selectedNode.ConnectSocket;

                        float distance = Vector3.Distance(
                            selectedNode.ConnectSocket.position,
                            target.ConnectSocket.position
                        );
                        ropesa.ropeLength = distance + 1.5f;

                        StartCoroutine(InitRopeMeshNextFrame(ropesa));

                        selectedNode.Neighbours.Add(target);
                        target.Neighbours.Add(selectedNode);
                        OnPipelineChanged?.Invoke(target);
                        // Debug.Log("Connected");
                        // Rope ropesa = Instantiate(rope) as Rope;
                        // ropesa.transform.position = selectedNode.ConnectSocket.position;
                        // ropesa.startPoint = target.ConnectSocket;
                        // ropesa.endPoint = selectedNode.ConnectSocket;
                        // float distance = Vector3.Distance(selectedNode.ConnectSocket.position, target.ConnectSocket.position);
                        // ropesa.ropeLength = distance ;
                        // ropesa.GetComponent<RopeMesh>().InitializeComponents();
                        // ropesa.GetComponent<RopeMesh>().GenerateMesh();
                        // ropesa.FireEvent();
                        // selectedNode.Neighbours.Add(target);
                        // target.Neighbours.Add(selectedNode);
                        // OnPipelineChanged?.Invoke(target);
                        // // foreach (var a in selectedNode.Neighbours)
                        // //     Debug.Log("Selected neighbours: " + a);
                        // // foreach (var a in target.Neighbours)
                        // //     Debug.Log("Target neighbours: " + a);
                    }
                }
            }

        }
    }
    private IEnumerator InitRopeMeshNextFrame(Rope ropesa)
    {
        // Rope kendi Start/OnEnable/Update'lerini bir çalıştırsın
        yield return null;

        var mesh = ropesa.GetComponent<RopeMesh>();
        mesh.InitializeComponents();
        mesh.GenerateMesh();
    }




}
