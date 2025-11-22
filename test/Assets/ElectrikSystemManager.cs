using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectrikSystemManager : MonoBehaviour
{
    [SerializeField] private GameObject ropePrefab;
    private bool buildMode;
    private IElectrikOutput selectedOutput;
    private GameObject ropeSetupObject;
    void Start()
    {
        InputManager.Instance.OnBuildMode += BuildModeChecker;
        InputManager.Instance.OnMouseHoverAction += ConnectPipes;
    }

    private void ConnectPipes(bool obj)
    {
        if (buildMode && obj)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(ray,radius:0.3f,out RaycastHit hit, 100f))
            {
                if (hit.transform.TryGetComponent(out IElectrikOutput output))
                {
                    selectedOutput = output;
                }
            }
        }
        else
        {
            if (selectedOutput != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.SphereCast(ray,radius :0.3f,out RaycastHit hit, 100f))
                {
                    if (hit.transform.TryGetComponent(out IElectrikInput selectedInput) && selectedOutput != selectedInput)
                    {

                        if (selectedInput.IsAvaliable(selectedOutput))
                        {
                            ropeSetupObject = Instantiate(ropePrefab);
                            ropeSetupObject.transform.position = selectedOutput.OutputSocket.position;
                            Quaternion lookRotation = Quaternion.LookRotation((selectedInput.InputSocket.position - selectedOutput.OutputSocket.position).normalized);
                            ropeSetupObject.transform.rotation = lookRotation;
                            float distance = Vector3.Distance(selectedOutput.OutputSocket.position, selectedInput.InputSocket.position);
                            ropeSetupObject.transform.localScale = new Vector3(1, 1, distance);
                            selectedInput.ConnectWithOutput(selectedOutput);
                        }

                    }
                }
                selectedOutput = null;
            }
        }
    }

    private void BuildModeChecker(bool obj)
    {
        buildMode = obj;
    }
}
