using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isRunning = false;
    private GameObject[] targets;
    private float standTime = 0f;
    private Vector3 lastPosition = Vector3.zero;
    [SerializeField] private Material stopMaterial;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

    void Update()
    {
        if (!isRunning || agent == null || !agent.isOnNavMesh || !agent.enabled) return;

        if(!agent.pathPending && agent.hasPath && (agent.remainingDistance <= agent.stoppingDistance)) FindAvailableAndClosestTarget();        

        if(Vector3.Distance(transform.position, lastPosition) <= 0.005f)
        {
            if (!agent.pathPending && agent.hasPath && agent.remainingDistance > agent.stoppingDistance)
            {
                standTime += Time.deltaTime;
            }            
        } 
        else standTime = 0f;        

        if(standTime >= 3f)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && stopMaterial != null)
            {
                renderer.material = stopMaterial;
            }

            agent.enabled = true;


            StopSimulation();
        }

        lastPosition = transform.position;
    }

    private void StartSimulation()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;

        targets = GameObject.FindGameObjectsWithTag("Target");
        isRunning = true;        

        if (agent != null && agent.isOnNavMesh) agent.isStopped = false;        

        FindAvailableAndClosestTarget();
    }

    private void StopSimulation()
    {
        isRunning = false;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    private void FindAvailableAndClosestTarget()
    {
        if (targets == null || targets.Length == 0) return;

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;                

        foreach(GameObject target in targets)
        {
            if (target == null) continue;

            Vector3 distanceVector = target.transform.position - currentPosition;
            float distance = distanceVector.sqrMagnitude;

            if(distance < closestDistance)
            {
                NavMeshPath path = new NavMeshPath();
                bool found = agent.CalculatePath(target.transform.position, path);

                if(found && path.status == NavMeshPathStatus.PathComplete)
                {
                    closest = target; 
                    closestDistance = distance;
                }
            }
        }

        if (closest == null) return;        
        else agent.SetDestination(closest.transform.position);
    }
}
