using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Models;
using Things.Game.Player.States;
using Things.Game.Configs.PlayerConfig;
using Unity.Netcode;

namespace Things.Game.Player
{
    public class PlayerPresenter : NetworkBehaviour, IUpdatable
    {
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerAnimationController animationController;
        
        [Header("Configs")]
        [SerializeField] private PlayerMovementConfig movementConfig;
        [SerializeField] private PlayerStatsConfig statsConfig;
        
        private PlayerStateMachine stateMachine;
        public HealthModel Health { get; private set; }

        public bool IsActive => gameObject.activeInHierarchy;

        // Exposed for states
        public CharacterController Controller => characterController;
        public PlayerAnimationController AnimController => animationController;
        public PlayerMovementConfig MovementConfig => movementConfig;
        public PlayerStatsConfig StatsConfig => statsConfig;
        public Vector3 MoveInput { get; private set; }

        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerHarvestState HarvestState { get; private set; }
        public PlayerCombatState CombatState { get; private set; }
        public PlayerSwimState SwimState { get; private set; }

        private void Awake()
        {
            Health = new HealthModel();
            float initialHealth = statsConfig != null ? statsConfig.MaxHealth : 100f;
            Health.Initialize(initialHealth);

            stateMachine = new PlayerStateMachine();
            
            IdleState = new PlayerIdleState(this, stateMachine);
            MoveState = new PlayerMoveState(this, stateMachine);
            HarvestState = new PlayerHarvestState(this, stateMachine);
            CombatState = new PlayerCombatState(this, stateMachine);
            SwimState = new PlayerSwimState(this, stateMachine);

            stateMachine.Initialize(IdleState);
        }

        private void OnEnable()
        {
            UpdateManager.Register(this, UpdatePhase.Normal);
        }

        private void OnDisable()
        {
            UpdateManager.Unregister(this);
        }

        public void OnUpdate(float deltaTime)
        {
            ReadInput();
            stateMachine.Update(deltaTime);
        }

        private void ReadInput()
        {
            if (!IsOwner) return;

            // Tạm thời lấy input từ Unity Input Manager cũ
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            MoveInput = new Vector3(h, 0f, v).normalized;
        }

        public void TransitionTo(PlayerStateBase newState)
        {
            stateMachine.ChangeState(newState);
        }
    }
}
