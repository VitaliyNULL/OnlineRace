using System.Collections;
using UnityEngine;

namespace VitaliyNULL.Player
{
    public class MaterialChanger : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private Material _mainMaterial;
        [SerializeField] private Material _lowAlphaMaterial;
        [SerializeField] private MeshRenderer _carBody;
        [SerializeField] private MeshRenderer _frontLeftWheel;
        [SerializeField] private MeshRenderer _frontRightWheeel;
        [SerializeField] private MeshRenderer _rearLeftWheel;
        [SerializeField] private MeshRenderer _rearRightWheel;

        #endregion

        #region Public Methods

        public void ChangeMaterialToLowAlpha()
        {
            SetMaterial(_lowAlphaMaterial);
        }

        #endregion



        #region PrivateMethods

        private void SetMaterial(Material material)
        {
            _carBody.material = material;
            _frontLeftWheel.material = material;
            _frontRightWheeel.material = material;
            _rearLeftWheel.material = material;
            _rearRightWheel.material = material;
        }

        #endregion
    }
}