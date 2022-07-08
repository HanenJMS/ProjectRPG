using RPG.Combat;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour, ISaveable
    {
        public static CameraController instance;

        [SerializeField] Transform cameraTransform;
        public Transform followTransform;

        [SerializeField] float normalSpeed;
        [SerializeField] float fastSpeed;
        [SerializeField] float movementSpeed;
        [SerializeField] float movementTime;
        [SerializeField] float rotationAmount;
        [SerializeField] Vector3 zoomAmount;

        [SerializeField] Quaternion newRotation;
        [SerializeField] Vector3 newPosition;
        [SerializeField] Vector3 newZoom;

        [SerializeField] Vector3 dragStartPosition;
        [SerializeField] Vector3 dragCurrentPosition;
        [SerializeField] Vector3 rotateStartPosition;
        [SerializeField] Vector3 rotateCurrentPosition;
        //methods
        //HandleMovementInput(); Takes in user input for moving the camera rig.

        private void Start()
        {
            instance = this;
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        private void Update()
        {
            if(followTransform != null)
            {
                transform.position = followTransform.position;
            }
            else
            {
                HandleMouseInput();
                HandleMovementInput();
                cameraMover();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                newPosition = transform.position;
                newRotation = transform.rotation;
                newZoom = cameraTransform.localPosition;
                followTransform = null;
            }
        }
        void HandleMovementInput()
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }

            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if(Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }

            if(Input.GetKey(KeyCode.F))
            {
                newZoom += -zoomAmount;
            }

            
        }
        void HandleMouseInput()
        {
            if(Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
            if(Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray,out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }

            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }

            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }

            if(Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }
        private void cameraMover()
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, movementTime * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTime * Time.deltaTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, movementTime * Time.deltaTime);
        }

        public object CaptureState()
        {
            return followTransform;
        }

        public void RestoreState(object state)
        {
            Transform follow = (Transform)state;
            followTransform = follow;
        }
    }
}
