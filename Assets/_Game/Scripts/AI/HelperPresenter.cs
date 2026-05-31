using UnityEngine;
using Things.Core.Infrastructure;

namespace Things.Game.AI
{
    public class HelperPresenter : MonoBehaviour, IUpdatable
    {
        private HelperStateMachine stateMachine;

        public bool IsActive => gameObject.activeInHierarchy;

        private void Awake()
        {
            stateMachine = new HelperStateMachine(this);
            // stateMachine.ChangeState(new GatheringState(this));
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
            stateMachine.Update(deltaTime);
        }

        public void MoveTo(Vector3 position)
        {
            // NavMeshAgent logic
        }
    }
}
