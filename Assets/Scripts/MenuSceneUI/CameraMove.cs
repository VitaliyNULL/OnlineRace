using System;
using System.Collections;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI
{
    public class CameraMove : MonoBehaviour
    {
        private readonly string CAR_SKIN = "CAR_SKIN";
        [SerializeField] private CarSelectButton carSelectButton;
        private float _moveForce = 20f;
        private bool _isMoving = false;
        private int _carIndex;

        private void Start()
        {
            if (PlayerPrefs.HasKey(CAR_SKIN))
            {
                _carIndex = PlayerPrefs.GetInt(CAR_SKIN);
                carSelectButton.SetCar(_carIndex);
                transform.position = new Vector3(transform.position.x + _moveForce * _carIndex,transform.position.y,transform.position.z);
            }
            else
            {
                _carIndex = 0;
                carSelectButton.SetCar(0);
            }
          
        }

        public void MoveLeft()
        {
            Debug.Log("Click");
            Debug.Log($"{transform.position.x} <= 0 is {transform.position.x <= 0}");
            Debug.Log($"Is moving is {_isMoving}");
            Debug.Log($"{transform.position.x} > -140f is {transform.position.x > -140f}");
            if (transform.position.x <= 0 && !_isMoving && transform.position.x > -140f)
            {
                _isMoving = true;
                Debug.Log("MoveLeft");
                StartCoroutine(StartMoveLeft());
               
            }
        }

        public void MoveRight()
        {
            Debug.Log("Click");
            if (transform.position.x >= -140f && !_isMoving && transform.position.x < 0)
            {
                _isMoving = true;
                Debug.Log("MoveRight");
                StartCoroutine(StartMoveRight());
                
            }
        }

        private IEnumerator StartMoveLeft()
        {
            float toMove = transform.position.x - _moveForce;
            while (transform.position.x >= toMove)
            {
                Debug.Log(transform.position.x + " " + toMove);
                transform.Translate(-_moveForce * Time.deltaTime, 0, 0);
                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(toMove, transform.position.y, transform.position.z);
            _isMoving = false;
            _carIndex--;
            carSelectButton.SetCar(_carIndex);
        }

        private IEnumerator StartMoveRight()
        {
            float toMove = transform.position.x + _moveForce;
            while (transform.position.x <= toMove)
            {
                Debug.Log(transform.position.x + " " + toMove);
                transform.Translate(_moveForce * Time.deltaTime, 0, 0);
                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(toMove, transform.position.y, transform.position.z);
            _isMoving = false;
            _carIndex++;
            carSelectButton.SetCar(_carIndex);
        }
    }
}