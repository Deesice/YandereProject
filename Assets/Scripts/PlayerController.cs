using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;    
    public float speed = 1;
    public float rotateSpeed = 1;
    Animator animator;

    Vector3 deltaPos;
    Vector3 camForwardProject;
    float magnitude;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        camForwardProject = cam.forward;
        camForwardProject.y = 0;
        camForwardProject.Normalize();
        deltaPos = Input.GetAxis("Horizontal") * Vector3.Cross(Vector3.up, camForwardProject) + Input.GetAxis("Vertical") * camForwardProject;
        deltaPos = Vector3.ClampMagnitude(deltaPos, 1) * speed;
        magnitude = deltaPos.magnitude;
        animator.SetFloat("speed", magnitude);
        if (magnitude > 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(deltaPos), Time.deltaTime * rotateSpeed);
            transform.localPosition += transform.forward * magnitude * Time.deltaTime;
        }
    }
}
