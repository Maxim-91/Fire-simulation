using UnityEngine;

public class Fire : MonoBehaviour
{
    private Vector3[] directions = {Vector3.back, Vector3.forward, Vector3.left, Vector3.right};
    [SerializeField] private float rayCastDistance = 1f;
    [SerializeField] private GameObject fire;

    void OnEnable()
    {
        SimulationManager.StartSimulation += StartSimulation;
    }

    void OnDisable()
    {
        SimulationManager.StartSimulation -= StartSimulation;
    }

    public void StartSimulation()
    {
        InvokeRepeating("FireSphread", SimulationManager.SimulationInterval, SimulationManager.SimulationInterval);
    }

    private void FireSphread()
    {
        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;

            if(!Physics.Raycast(transform.position, direction, out hit, rayCastDistance))
            {
                Vector3 newPosition = transform.position + (direction * rayCastDistance);
                Instantiate(fire, newPosition, Quaternion.identity).GetComponent<Fire>().StartSimulation();
            }
        }
    }
}
