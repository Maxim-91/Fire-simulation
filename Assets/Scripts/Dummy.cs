using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private bool isRunning = false;
    private GameObject[] targets;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void OnEnable()
    {
        SimulationManager.StartSimulation += StartSimulation;
        SimulationManager.StopSimulation += StopSimulation;
    }

    void OnDisable()
    {
        SimulationManager.StartSimulation -= StartSimulation;
        SimulationManager.StopSimulation -= StopSimulation;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSimulation()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        isRunning = true;
        FindClosestTarget();
    }

    private void StopSimulation()
    {
        isRunning = false;
    }

    private void FindClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = Mathf. Infinity;
        Vector3 currentPosition = transform.position;

        foreach(GameObject target in targets)
        {
            Vector3 distanceVector = target.transform.position - currentPosition;
            float distance = distanceVector.sqrMagnitude;

            if(distance < closestDistance)
            {
                closest = target;
            }
        }

        agent.SetDestination (closest.transform.position);
    }
}
