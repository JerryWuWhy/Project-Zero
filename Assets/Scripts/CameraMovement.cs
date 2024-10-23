using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS
{
    public class CameraMovement : MonoBehaviour
    {
        public Camera gameCamera;
        public float cameraMovementSpeed = 5;
        public float speedReducement = 7f;
        private float _speed;
        private void Start()
        {
            gameCamera = GetComponent<Camera>();
        }
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    _speed = -touch.deltaPosition.x * cameraMovementSpeed * Time.deltaTime;
                }
                else
                {
                    ReduceSpeed();
                }
            }
            else
            {
                ReduceSpeed();
            }
            gameCamera.transform.position += Time.deltaTime * _speed * Vector3.right;
        }
        private void ReduceSpeed()
        {
            _speed = Mathf.Lerp(_speed, Input.GetAxis("Horizontal"), speedReducement * Time.unscaledDeltaTime);
        }
    }
}