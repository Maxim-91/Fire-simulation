using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private Slider slider;
    
    void Start()
    {
        if (slider != null)
        {
            SpawnObjects(Mathf.RoundToInt(slider.value));
        }
    }

    public void SliderValueChanged(float value)
    {
        SpawnObjects(Mathf.RoundToInt(value));
    }

    private void SpawnObjects(int count)
    {
        DestroyObjects();

        if (spawnObject == null) return;

        for(int i = 0; i < count; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * radius;
            randomPosition.y = 0.9f;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas))
            {
                Instantiate(spawnObject, hit.position, Quaternion.identity);
            }
        }
    }

    private void DestroyObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Dummy");

        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
