using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayCaster : MonoBehaviour
{
    private List<Ray> rays;

    void Awake() {
        rays = new List<Ray>();
    }
    
    public bool LaunchRays() {
        rays.Clear();
        rays.Add(new Ray(transform.position, transform.forward));
        
        RaycastHit hit;

        foreach (var ray in rays) {
            if (Physics.Raycast(ray, out hit, 10)) {print(hit.collider);
                if (hit.collider.CompareTag("Player")) {
                    print("muere lazor");
                    return true;
                }
               
            }
        }

        return false;
    }
    
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.green;
        Vector3 direction = transform.forward * 5;
        Gizmos.DrawRay(transform.position, direction);

    }
}
