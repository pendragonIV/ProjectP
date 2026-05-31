using UnityEngine;
using System.Collections;
using Things.Game.Configs.Resource;
using Things.Game.Events;
using Things.Core.Infrastructure;

namespace Things.Game.Resource
{
    public class ResourceSourcePresenter : MonoBehaviour
    {
        [SerializeField] private ResourceSourceConfig config;
        
        private int hitsRemaining;
        private bool isDepleted;
        private Coroutine respawnCoroutine;

        private void Awake()
        {
            hitsRemaining = config.MaxHits;
        }

        public void GetHit()
        {
            if (isDepleted) return;

            hitsRemaining--;
            EventBus.Publish(new ResourceHarvestedEvent { SourceId = config.SourceId, CurrencyId = config.DroppedCurrencyId });

            if (hitsRemaining <= 0)
            {
                isDepleted = true;
                
                // Khởi chạy coroutine TRƯỚC khi tắt visual, coroutine vẫn chạy
                // vì chúng ta chỉ tắt visual (renderer + collider) thay vì SetActive(false)
                SetVisualActive(false);
                respawnCoroutine = StartCoroutine(RespawnAfterDelay(config.RespawnDuration));
            }
        }

        private IEnumerator RespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Respawn();
        }

        private void Respawn()
        {
            hitsRemaining = config.MaxHits;
            isDepleted = false;
            SetVisualActive(true);
            respawnCoroutine = null;
        }

        /// <summary>
        /// Ẩn/hiện visual thay vì SetActive(false) để coroutine vẫn chạy được.
        /// </summary>
        private void SetVisualActive(bool active)
        {
            var renderer = GetComponentInChildren<Renderer>();
            if (renderer != null) renderer.enabled = active;
            
            var collider = GetComponent<Collider>();
            if (collider != null) collider.enabled = active;
        }

        private void OnDestroy()
        {
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
            }
        }
    }
}
