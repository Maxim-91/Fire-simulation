using UnityEngine;
using System;

public class SimulationManager : MonoBehaviour
{
    public static event Action StartSimulation;
    public static event Action StopSimulation;

    public static float SimulationInterval { get; private set; } = 2f;

    public void StartButtonClicked()
    {
        StartSimulation?.Invoke();
    }
}
