using UnityEngine;

public class LineTest : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            lineRenderer.SetPosition(1,hitPoint);
        }
    }
}
