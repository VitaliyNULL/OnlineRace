using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class RotateCar : MonoBehaviour
    {
        #region Private Fields

        private readonly float _toRotate = 20f;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            transform.Rotate(0, _toRotate * Time.deltaTime,
                0, Space.Self);
        }

        #endregion
    }
}