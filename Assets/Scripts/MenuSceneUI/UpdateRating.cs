using TMPro;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.MenuSceneUI
{
    public class UpdateRating : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ratingText;

        private void Awake()
        {
            FirebaseManager.FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager.FirebaseManager>();
            firebaseManager.InitializeFirebase();
            firebaseManager.LoadData(ChangeTextRating);
        }

        private void ChangeTextRating()
        {
            _ratingText.text = PlayerPrefs.GetInt(ConstKeys.Rating).ToString();
        }
    }
}