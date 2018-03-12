using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {

        public Transform target;
        [SerializeField] float distance = 5.0f;
        [SerializeField] float xSpeed = 120.0f;
        [SerializeField] float ySpeed = 120.0f;

        [SerializeField] float yMinLimit = 0f;
        [SerializeField] float yMaxLimit = 80f;

        [SerializeField] float distanceMin = .5f;
        [SerializeField] float distanceMax = 15f;

        Cursor cursor;
        Rigidbody rigidbody;

        float x = 0.0f;
        float y = 0.0f;

        // Use this for initialization
        void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
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

        void LateUpdate()
        {
            if (target)
            {
                x += Input.GetAxis("XBox_RStickX") * xSpeed * distance * 0.02f;
                y += Input.GetAxis("XBox_RStickY") * ySpeed * 0.02f;

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

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
