using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.FusionManager;
using VitaliyNULL.InGameUI;
using VitaliyNULL.MapGeneration;
using VitaliyNULL.MenuSceneUI.Garage;
using VitaliyNULL.Props;

namespace VitaliyNULL.Player
{
    public class Player : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private LayerMask _mapTileLayer;
        [SerializeField] private LayerMask _propLayer;
        private MaterialChanger _materialChanger;
        [HideInInspector] public GameManager gameManager;
        private bool _isInvulnerable;
        [SerializeField] public PlayerMove playerMove;
        [SerializeField] private List<Car> _cars;
        [Networked] public bool finished { get; set; }

        private short _currentPosition;
        private GameUI _gameUI;
        private EndRaceUI _endRaceUI;

        #endregion

        #region Public Properties

        public short CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                if (HasInputAuthority)
                {
                    _gameUI.UpdatePlayerPosition(_currentPosition);
                }
            }
        }

        public bool IsInvulnerable
        {
            get => _isInvulnerable;
            set
            {
                _isInvulnerable = value;
                if (value)
                {
                    StartInvulnerability();
                }
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_SetCar(PlayerPrefs.GetInt(ConstKeys.CarSkin));
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            RaycastHit raycastHit;

            if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, 3, _mapTileLayer))
            {
                MapTile mapTile = raycastHit.collider.GetComponent<MapTile>();
                mapTile.StartChainToActiveTile(mapTile, 0);
                mapTile.StartDisablingTile(mapTile, 0);
            }

            var colliders =
                Physics.OverlapBox(transform.position, new Vector3(3, 2, 7), Quaternion.identity, _propLayer);
            if (colliders != null)
            {
                // RPC_InteractWithProp(prop, this);
                foreach (var collider in colliders)
                {
                    collider.GetComponent<Prop>().Interact(this);
                }
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator WaitForInvulnerability()
        {
            yield return new WaitForSeconds(2f);
            IsInvulnerable = false;
        }

        #endregion

        #region Private Methods

        private void StartInvulnerability()
        {
            StartCoroutine(WaitForInvulnerability());
        }

        #endregion

        #region Public Methods

        public void OpenFinishUI()
        {
            _endRaceUI.ShowEndRaceUI();
            GameTime gameTime = gameManager.GetGameTime();
            _endRaceUI.InitTimeAndScore(gameTime.ToString(), (10000 / gameTime.GetTime()).ToString());
        }

        public void InitGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            _gameUI = gameManager.gameUI;
            _endRaceUI = gameManager.endRaceUI;
        }

        #endregion

        #region RPC

        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        public void RPC_Finish()
        {
            Debug.Log("You win");
            finished = true;
        }

        [Rpc]
        private void RPC_SetCar(int index)
        {
            foreach (var car in _cars)
            {
                if ((int)car.carEnum == index)
                {
                    car.gameObject.SetActive(true);
                    _materialChanger = car.GetComponent<MaterialChanger>();
                    break;
                }
            }


            if (!HasInputAuthority)
            {
                _materialChanger.ChangeMaterialToLowAlpha();
            }
            else
            {
                CinemachineVirtualCamera Camera = FindObjectOfType<CinemachineVirtualCamera>();
                Transform target = _materialChanger.transform;
                Camera.Follow = target;
                Camera.LookAt = target;
            }
        }

        [Rpc]
        private void RPC_ChangeToLowAlpha()
        {
            if (!HasInputAuthority)
            {
                _materialChanger.ChangeMaterialToLowAlpha();
            }
        }

        #endregion
    }
}