using UnityEngine;
using Things.Core.Infrastructure;
using System.Collections;

namespace Things.Game.Services.Audio
{
    public class AudioService : IAudioService, System.IDisposable
    {
        private readonly AudioSource musicSource;
        private readonly Transform sfxRoot;
        private readonly ObjectPool<AudioSource> sfxPool;
        private readonly GameObject serviceRoot;
        private readonly CoroutineRunner runner;

        public AudioService()
        {
            serviceRoot = new GameObject("[AudioService]");
            Object.DontDestroyOnLoad(serviceRoot);

            musicSource = serviceRoot.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0f;

            sfxRoot = new GameObject("SFX_Root").transform;
            sfxRoot.SetParent(serviceRoot.transform);

            var sfxPrefabGo = new GameObject("SFX_Prefab");
            sfxPrefabGo.SetActive(false);
            sfxPrefabGo.transform.SetParent(serviceRoot.transform);
            var sfxPrefab = sfxPrefabGo.AddComponent<AudioSource>();
            
            sfxPool = new ObjectPool<AudioSource>(sfxPrefab, sfxRoot, 5);
            runner = serviceRoot.AddComponent<CoroutineRunner>();
        }

        public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) return;
            var source = sfxPool.Get();
            source.transform.position = position;
            source.spatialBlend = 1f; // 3D
            PlayAndRelease(source, clip, volume);
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            var source = sfxPool.Get();
            source.spatialBlend = 0f; // 2D
            PlayAndRelease(source, clip, volume);
        }

        private void PlayAndRelease(AudioSource source, AudioClip clip, float volume)
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
            runner.StartCoroutine(ReleaseAfterPlay(source, clip.length));
        }

        private IEnumerator ReleaseAfterPlay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            sfxPool.Release(source);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            if (musicSource.clip == clip && musicSource.isPlaying) return;

            musicSource.clip = clip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void Dispose()
        {
            if (serviceRoot != null)
            {
                Object.Destroy(serviceRoot);
            }
        }

        private class CoroutineRunner : MonoBehaviour { }
    }
}
