using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Vector3 fireLocation = hit.point + Vector3.up * 0.01f;
                Instantiate(fire, fireLocation, Quaternion.identity);
            }
        }
    }
}
