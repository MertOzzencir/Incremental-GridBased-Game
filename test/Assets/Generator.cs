using UnityEngine;

public class Generator : MonoBehaviour, IElectrikOutput
{
    [SerializeField] private float powerGenerationPerSecond;
    [SerializeField] private Transform outputSocket;
    public static float TotalEnergyPerSecond;

    public Transform OutputSocket { get => outputSocket; set => outputSocket = value; }

    void Awake()
    {
        TotalEnergyPerSecond = powerGenerationPerSecond;
    }

    void Update()
    {
        //Debug.Log(TotalEnergyPerSecond);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }


}
