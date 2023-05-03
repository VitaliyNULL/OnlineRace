using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LeaderBoard
{
    public class LeaderBoardContent : MonoBehaviour
    {
        [SerializeField] private LeaderBoardItem leaderBoardItem;

        private void OnEnable()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Debug.Log("LoadingLeaderBoard");
            FindObjectOfType<FirebaseManager.FirebaseManager>().LoadLeaderBoard(this);
        }

        public void InstantiateLeaderBoardItem(string username, string rating)
        {
            Debug.Log("InstantiatedItem");
            LeaderBoardItem leaderBoardItem =
                Instantiate(this.leaderBoardItem,transform);
            leaderBoardItem.Init(username, rating);
            Debug.Log("Initialized");
        }
    }
}