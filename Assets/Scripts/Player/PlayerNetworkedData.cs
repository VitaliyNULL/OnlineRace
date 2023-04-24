using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.InGameUI;

namespace VitaliyNULL.Player
{
    public class PlayerNetworkedData : NetworkBehaviour
    {
        #region Private Fields

        [Networked(OnChanged = nameof(OnPlayerUpdateNickName))]
        private NetworkString<_16> _name { get; set; }

        [Networked(OnChanged = nameof(OnPlayerUpdateDistance))]
        private float _distance { get; set; }

        [Networked(OnChanged = nameof(OnPlayerUpdatePosition))]
        private short _position { get; set; }

        private GameUI _gameUI;

        #endregion

        #region MonoBehaviour Callbacks

        public override void Spawned()
        {
            _gameUI = FindObjectOfType<GameUI>();
            if (HasInputAuthority)
            {
                RPC_SetNickName(PlayerPrefs.GetString(ConstKeys.UsernameKey));
            }
        }

        #endregion

        #region Private Methods

        private static void OnPlayerUpdateNickName(Changed<PlayerNetworkedData> changed)
        {
            changed.Behaviour._gameUI.UpdatePlayerNickName(changed.Behaviour._name.ToString());
        }

        private static void OnPlayerUpdateDistance(Changed<PlayerNetworkedData> changed)
        {
            changed.Behaviour._gameUI.UpdatePlayerDistance(changed.Behaviour._distance);
        }

        private static void OnPlayerUpdatePosition(Changed<PlayerNetworkedData> changed)
        {
            changed.Behaviour._gameUI.UpdatePlayerPosition(changed.Behaviour._position);
        }

        #endregion

        #region Public Methods

        public void UpdateDistance(float distance)
        {
            _distance = distance;
        }

        public void UpdatePosition(short position)
        {
            _position = position;
        }

        #endregion

        #region RPC

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RPC_SetNickName(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            _name = name;
        }

        #endregion
    }
}