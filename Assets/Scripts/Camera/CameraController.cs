using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera zoom values:")]
    public bool startZoom = false; // controls when zoom can be activated
    public float maxZoomDistance = 5;  // the max value can camera reach when zoom in
    public Vector3 offset = new Vector3(0, 0, -10); // offset values to adjust camera positions
    public Transform targetNPC; // the NPC object to focus him

    private float maxStartZoomDistance; // the max value can camera reach when zoom out
    private float smoothTime = 0.25f; // to smooth camera position movement
    private float speedZoom = 0.05f; // to smooth camera zoom movement
    private Vector3 vel = Vector3.zero; // ref velocity values
    private Vector3 initialPos; // the start position camera inside the game

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        // receives the actual camera zoom value
        maxStartZoomDistance = cam.orthographicSize;

        // receives the actual position of camera
        initialPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            startZoom = !startZoom;
        }
    }

    private void FixedUpdate()
    {               
        if (startZoom)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        // lerp between camera zoom values
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, maxZoomDistance, speedZoom);

        // set the position of camera to NPC smoothly
        Vector3 targetPos = targetNPC.position + offset;
        cam.transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, smoothTime);
    }

    public void ZoomOut()
    {
        // lerp between camera zoom values
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, maxStartZoomDistance, speedZoom);

        // set the position of camera to Player smoothly
        cam.transform.position = Vector3.SmoothDamp(transform.position, initialPos, ref vel, smoothTime);
    }
}
