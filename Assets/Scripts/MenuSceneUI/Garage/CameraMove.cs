using System.Collections;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class CameraMove : MonoBehaviour
    {
        #region Private Fields

        private readonly string _carSkin = "CAR_SKIN";
        [SerializeField] private CarSelectButton _carSelectButton;
        private float _moveForce = 20f;
        private bool _isMoving;
        private int _carIndex;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (PlayerPrefs.HasKey(_carSkin))
            {
                _carIndex = PlayerPrefs.GetInt(_carSkin);
                _carSelectButton.SetCar(_carIndex);
                transform.position = new Vector3(transform.position.x + _moveForce * _carIndex, transform.position.y,
                    transform.position.z);
            }
            else
            {
                _carIndex = 0;
                _carSelectButton.SetCar(0);
            }
        }

        #endregion

        #region Public Methods

        public void MoveLeft()
        {
            if (transform.position.x <= 0 && !_isMoving && transform.position.x > -140f)
            {
                _isMoving = true;
                StartCoroutine(StartMoveLeft());
            }
        }

        public void MoveRight()
        {
            if (transform.position.x >= -140f && !_isMoving && transform.position.x < 0)
            {
                _isMoving = true;
                StartCoroutine(StartMoveRight());
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator StartMoveLeft()
        {
            float toMove = transform.position.x - _moveForce;
            while (transform.position.x >= toMove)
            {
                transform.Translate(-_moveForce * Time.deltaTime, 0, 0);
                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(toMove, transform.position.y, transform.position.z);
            _isMoving = false;
            _carIndex--;
            _carSelectButton.SetCar(_carIndex);
        }

        private IEnumerator StartMoveRight()
        {
            float toMove = transform.position.x + _moveForce;
            while (transform.position.x <= toMove)
            {
                transform.Translate(_moveForce * Time.deltaTime, 0, 0);
                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(toMove, transform.position.y, transform.position.z);
            _isMoving = false;
            _carIndex++;
            _carSelectButton.SetCar(_carIndex);
        }

        #endregion
    }
}