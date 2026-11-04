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

    private bool fireTouched = false;
    private bool finished = false;

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

        if(standTime >= 2f)
        {
            // 1. Fire touched Dummy
            if (fireTouched)
            {
                Die();
                return;
            }

            // 2. There is no available path to Target.
            if (!HasReachableTarget())
            {
                Die();
                return;
            }
            // 3. If there is a path, there might be a traffic jam.
            else standTime = 0f;

            
            //Renderer renderer = GetComponent<Renderer>();
            //if (renderer != null && stopMaterial != null)  renderer.material = stopMaterial;
            //agent.enabled = true;
            //StopSimulation();
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

    private bool HasReachableTarget()
    {
        if (targets == null || targets.Length == 0) return false;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            NavMeshPath path = new NavMeshPath();

            if (agent.CalculatePath(target.transform.position, path) && path.status == NavMeshPathStatus.PathComplete) return true;            
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            fireTouched = true;
            Die();
        }

        if (other.CompareTag("Target"))
        {
            Renderer targetRenderer = other.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                Renderer renderer = GetComponent<Renderer>();

                if (renderer != null)
                    renderer.material = targetRenderer.material;
            }

            if (!finished)
            {
                finished = true;
                SimulationManager.RegisterSaved();

                agent.enabled = false;

                // The Dummy rises above the Target and disappears.
                transform.position += Vector3.up * 2f;
                Destroy(gameObject, 1.5f);
            }
        }
    }

    private void Die()
    {
        if (finished) return;

        finished = true;        

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null && stopMaterial != null)
            renderer.material = stopMaterial;

        if (agent != null)
            agent.enabled = false;        

        SimulationManager.RegisterDead();

        StopSimulation();

        //if (agent != null && agent.isOnNavMesh)
        //{
            //agent.isStopped = true;
        //}
    }
}
