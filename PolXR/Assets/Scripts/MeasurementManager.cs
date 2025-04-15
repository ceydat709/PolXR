using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasurementManager : MonoBehaviour
{
    public GameObject spherePrefab;
    public GameObject linePrefab;
    public GameObject distanceTextPrefab; // This is your MeasurementLabel prefab

    private GameObject sphereA;
    private GameObject sphereB;
    private GameObject line;
    private GameObject distanceText;

    private LineRenderer lineRenderer;
    private TextMeshProUGUI distanceTextUI;

    void Start()
    {
        // Find user's view and compute offset to spawn spheres bottom right
        Transform cameraTransform = Camera.main.transform;
        Vector3 offsetRight = cameraTransform.right * 0.3f;
        Vector3 offsetDown = cameraTransform.up * -0.2f;
        Vector3 offsetForward = cameraTransform.forward * 0.5f;
        Vector3 spawnPosition = cameraTransform.position + offsetForward + offsetRight + offsetDown;

        // Instantiate measurement objects
        sphereA = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        sphereB = Instantiate(spherePrefab, spawnPosition + new Vector3(0.1f, 0, 0), Quaternion.identity);
        line = Instantiate(linePrefab);
        distanceText = Instantiate(distanceTextPrefab);

        // Name for clarity
        sphereA.name = "SphereA";
        sphereB.name = "SphereB";
        line.name = "MeasurementLine";
        distanceText.name = "DistanceText";

        // Get references
        lineRenderer = line.GetComponent<LineRenderer>();

        // This line is the key fix: find the nested DistanceLabel inside the Canvas of your MeasurementLabel prefab
        distanceTextUI = distanceText.transform.Find("Canvas/DistanceLabel").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (sphereA == null || sphereB == null || lineRenderer == null || distanceTextUI == null)
            return;

        // Draw line
        lineRenderer.SetPosition(0, sphereA.transform.position);
        lineRenderer.SetPosition(1, sphereB.transform.position);

        // Calculate and update distance
        float distance = Vector3.Distance(sphereA.transform.position, sphereB.transform.position);
        distanceTextUI.text = $"{distance:F2} meters";

        // Position label above midpoint
        Vector3 midpoint = (sphereA.transform.position + sphereB.transform.position) / 2;
        distanceText.transform.position = midpoint + new Vector3(0, 0.05f, 0);

        // Face the camera
        distanceText.transform.LookAt(Camera.main.transform);
        distanceText.transform.Rotate(0, 180f, 0);
    }
}
