using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Models;
using Things.Game.Configs.EnemyConfig;
namespace Things.Game.Enemies
{
    public class EnemyPresenter : MonoBehaviour, IUpdatable
    {
        [SerializeField] private EnemyDefinition definition;
        [SerializeField] private EnemyView view;

        private HealthModel health;
        
        public bool IsActive => gameObject.activeInHierarchy && !health.IsDepleted;

        private void Awake()
        {
            health = new HealthModel();
            health.Initialize(definition.MaxHealth);
            health.OnDepleted += Die;
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
            // Thay vì tự gọi Update() liên tục kể cả khi chết, giờ sẽ được UpdateManager quản lý
            // Logic AI đuổi theo Player
            
            // view.UpdateMovementAnimation(speed);
        }

        public void TakeDamage(float amount)
        {
            if (health.IsDepleted) return;
            health.Subtract(amount);
        }

        private void Die()
        {
            view.PlayDeathAnimation();
            UpdateManager.Unregister(this);
            
            // Drop loot
            // ServiceLocator.Get<IPoolManager>().Spawn(definition.DroppedCurrencyId, transform.position);
            
            Destroy(gameObject, 2f);
        }
    }
}
