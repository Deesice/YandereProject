using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float maxVerticalAngleInDegrees = 80;
    public Transform anchor;
    public float orbitRadius;
    public float mouseSensitivity;
    public bool inverseY;
    public float additionalRotationWeight;
    public float additionalRotationSpeed;
    public float offsetMagnitude;
    public float offsetAngle;
    public float positionSpeed;

    Vector3 targetPos;
    Vector3 smoothAnchorPos;
    Vector3 myFuturePos;
    Vector2 cumulativeAngleInRadians;
    Quaternion additionalRotation;
    private void Awake()
    {
        maxVerticalAngleInDegrees *= Mathf.Deg2Rad;
        offsetAngle *= Mathf.Deg2Rad;
    }
    void LateUpdate()
    {
        cumulativeAngleInRadians.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        cumulativeAngleInRadians.y = Mathf.Clamp(cumulativeAngleInRadians.y + Input.GetAxis("Mouse Y") * mouseSensitivity, -maxVerticalAngleInDegrees, maxVerticalAngleInDegrees);

        smoothAnchorPos = Vector3.Lerp(smoothAnchorPos, anchor.position, Time.unscaledDeltaTime * positionSpeed);
        targetPos.y = 0;
        targetPos.x = Mathf.Sin(cumulativeAngleInRadians.x + offsetAngle);
        targetPos.z = Mathf.Cos(cumulativeAngleInRadians.x + offsetAngle);
        targetPos *= offsetMagnitude;
        targetPos += smoothAnchorPos;

        myFuturePos.x = Mathf.Sin(cumulativeAngleInRadians.x);
        myFuturePos.y = inverseY ? Mathf.Sin(cumulativeAngleInRadians.y) : -Mathf.Sin(cumulativeAngleInRadians.y);
        myFuturePos.z = Mathf.Cos(cumulativeAngleInRadians.x);
        myFuturePos.Normalize();
        myFuturePos *= orbitRadius;
        myFuturePos += targetPos;

        additionalRotation = Quaternion.Lerp(additionalRotation,
            Quaternion.LerpUnclamped(Quaternion.identity, anchor.localRotation, additionalRotationWeight),
            additionalRotationSpeed * Time.unscaledDeltaTime);

        transform.SetPositionAndRotation(myFuturePos,
            Quaternion.LookRotation(targetPos - myFuturePos, Vector3.up) * additionalRotation);
    }
}
