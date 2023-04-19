using UnityEngine;

namespace VitaliyNULL.Props
{
    public class PropRotation : MonoBehaviour
    {
        #region Private Fields

        private float _toRotate = 80f;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            transform.Rotate(0, _toRotate * Time.deltaTime,
                0, Space.World);
        }

        #endregion
    }
}