using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private bool isRunning = false;
    private GameObject[] targets;   
    private float standTime = 0f;
    private Vector3 lastPosition = Vector3.zero; 
    [SerializeField] private Material stopMaterial;

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
        if (!isRunning) return;

        if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance)) FindAvailableAndClosestTarget();
        
        if(Vector3.Distance(transform.position, lastPosition) <= 0.005f) standTime += Time.deltaTime;
        else standTime = 0;        

        if(standTime >= 2f)
        {
            GetComponent<Renderer>().material = stopMaterial;            
            agent.enabled = true;
            StopSimulation();
        }

        lastPosition = transform.position;
    }

    private void StartSimulation()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        isRunning = true;
        FindAvailableAndClosestTarget();
    }

    private void StopSimulation()
    {
        isRunning = false;
    }

    private void FindAvailableAndClosestTarget()
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

        agent.SetDestination (closest.transform.position);
    }
}
