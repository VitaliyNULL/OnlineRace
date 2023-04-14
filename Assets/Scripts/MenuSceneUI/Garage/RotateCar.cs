using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class RotateCar : MonoBehaviour
    {
        private float toRotate = 20f;

        #region MonoBehaviour Callbacks

        private void Update()
        {
            transform.Rotate(0, toRotate * Time.deltaTime,
                0, Space.Self);
        }

        #endregion
    }
}