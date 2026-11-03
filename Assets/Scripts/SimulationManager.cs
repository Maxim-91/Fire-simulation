using UnityEngine;
using System;

public class SimulationManager : MonoBehaviour
{
    public static event Action StartSimulation;
    public static event Action StopSimulation;

    [SerializeField]private float simulationInterval;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        StartSimulation.Invoke();
    }
}
