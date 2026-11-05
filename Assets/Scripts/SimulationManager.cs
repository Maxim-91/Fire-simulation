using UnityEngine;
using System;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static event Action StartSimulation;
    public static event Action StopSimulation;

    public static float SimulationInterval { get; private set; } = 2f;

    private int savedCount = 0;
    private int deadCount = 0;

    [SerializeField] private TextMeshProUGUI savedText;
    [SerializeField] private TextMeshProUGUI deadText;

    private static SimulationManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartButtonClicked()
    {
        savedCount = 0;
        deadCount = 0;         

        StartSimulation?.Invoke();
    }

    public static void RegisterSaved()
    {
        if (instance == null) return;
        instance.savedCount++;
        if (instance.savedText != null) instance.savedText.text = "Survivors: " + instance.savedCount;
    }

    public static void RegisterDead()
    {
        if (instance == null) return;
        instance.deadCount++;
        if (instance.deadText != null) instance.deadText.text = "Dead: " + instance.deadCount;
    }
}
