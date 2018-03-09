using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraFollow : MonoBehaviour {

    //    GameObject player;

    //private Quaternion cameraRotation;
    //private Quaternion characterTargetRotation;
    //private Quaternion cameraTargetRotation;

    //public float XSensitivity = 2f;
    //public float YSensitivity = 2f;
    //public bool clampVerticalRotation = true;
    //public float MinimumX = -90F;
    //public float MaximumX = 90F;
    //public bool smooth;
    //public float smoothTime = 5f;

    public Transform target;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float xSpeed = 120.0f;
    [SerializeField] float ySpeed = 120.0f;

    [SerializeField] float yMinLimit = 0f;
    [SerializeField] float yMaxLimit = 80f;

    [SerializeField] float distanceMin = .5f;
    [SerializeField] float distanceMax = 15f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    //void Update()
    //{

    //}

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    //private void RotateView()
    //{
    //    LookRotation(player.transform, transform);
    //}

    //public void LookRotation(Transform character, Transform camera)
    //{
    //    float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
    //    float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

    //    characterTargetRotation *= Quaternion.Euler(0f, yRot, 0f);
    //    cameraTargetRotation *= Quaternion.Euler(-xRot, 0f, 0f);

    //    if (clampVerticalRotation)
    //        cameraTargetRotation = ClampRotationAroundXAxis(cameraRotation);

    //    if (smooth)
    //    {
    //        character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRotation,
    //            smoothTime * Time.deltaTime);
    //        camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRotation,
    //            smoothTime * Time.deltaTime);
    //    }
    //    else
    //    {
    //        character.localRotation = characterTargetRotation;
    //        camera.localRotation = cameraTargetRotation;
    //    }
    //}

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    //Quaternion ClampRotationAroundXAxis(Quaternion q)
    //{
    //    q.x /= q.w;
    //    q.y /= q.w;
    //    q.z /= q.w;
    //    q.w = 1.0f;

    //    float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

    //    angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

    //    q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

    //    return q;
    //}
}
