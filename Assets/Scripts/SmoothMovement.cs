using UnityEngine;

namespace VitaliyNULL
{
    public class SmoothMovement : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position,
                transform.position + Vector3.forward * speed * Time.deltaTime, 1);
        }
    }
}