using UnityEngine;

namespace VitaliyNULL.Props
{
    public class PropRotation : MonoBehaviour
    {
        private float toRotate = 80f;

        #region MonoBehaviour Callbacks

        private void Update()
        {
            transform.Rotate(0, toRotate * Time.deltaTime,
                0, Space.World);
        }

        #endregion
    }
}