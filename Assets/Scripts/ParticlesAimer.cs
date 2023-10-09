using UnityEngine;

public class ParticlesAimer : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var laser))
        {
            transform.LookAt(laser.point);
        }
    }
}
