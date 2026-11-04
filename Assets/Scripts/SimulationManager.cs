using UnityEngine;
using System;

public class SimulationManager : MonoBehaviour
{
    public static event Action StartSimulation;
    public static event Action StopSimulation;

    public static float SimulationInterval { get; private set; } = 2f;

    public static int savedCount = 0;
    public static int deadCount = 0;

    public void StartButtonClicked()
    {
        savedCount = 0;
        deadCount = 0;

        StartSimulation?.Invoke();
    }

    public static void RegisterSaved()
    {
        savedCount++;
        Debug.Log("savedCount: " + savedCount);
    }

    public static void RegisterDead()
    {
        deadCount++;
        Debug.Log("deadCount: " + deadCount);
    }
}
