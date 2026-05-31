using Things.Core.Infrastructure;
using UnityEngine;

namespace Things.Game.Services.Audio
{
    public interface IAudioService : IService
    {
        void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f);
        void PlaySFX(AudioClip clip, float volume = 1f);
        void PlayMusic(AudioClip clip);
        void StopMusic();
    }
}
