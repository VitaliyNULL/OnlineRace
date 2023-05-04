using UnityEngine;

namespace VitaliyNULL.Player
{
    public class SwipeController : MonoBehaviour
    {
        private Vector2 _startTouchPos;
        private Vector2 _swipeDelta;
        private float _minDeltaSwipe = 60f;
        private bool _isSwiping;

        //this needs for check what swipe is larger horizontal or vertical
        private float _horizontalSwipe;
        private float _verticalSwipe;

        public void Run()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwiping = true;
                _startTouchPos = Input.mousePosition;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }

            CheckSwipe();
        }

        protected void CheckSwipe()
        {
            _swipeDelta = Vector2.zero;
            if (_isSwiping)
            {
                _swipeDelta = Input.mousePosition;
            }

            _horizontalSwipe = Mathf.Abs(_swipeDelta.x - _startTouchPos.x);
            _verticalSwipe = Mathf.Abs(_swipeDelta.y - _startTouchPos.y);
            if (_horizontalSwipe > _minDeltaSwipe && _horizontalSwipe > _verticalSwipe)
            {
                if (_swipeDelta.x < _startTouchPos.x)
                {
                    //Go Left
                }
                else if (_swipeDelta.x > _startTouchPos.x)
                {
                    //Go right
                }

                ResetSwipe();
            }
        }

        protected void ResetSwipe()
        {
            _isSwiping = false;
            _startTouchPos = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }
    }
}