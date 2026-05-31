using UnityEngine;

namespace Things
{
    [StaticUnload]
    [RequireComponent(typeof(AudioSource))]
    public class MusicSource : MonoBehaviour
    {
        private const float DEFAULT_FADE_DURATION = 0.3f;

        private static MusicSource defaultMusicSource;

        private static MusicSource activeMusicSource;
        public static MusicSource ActiveMusicSource => activeMusicSource;

        [SerializeField] bool activateAutomatically = false;

        private AudioSource audioSource;
        public AudioSource AudioSource => audioSource;

        private TweenCase fadeTweenCase;

        private float volumeMultiplier = 1.0f;

        private void Awake()
        {
            if(activateAutomatically)
            {
                Init();
                Activate();
            }
        }

        public void Init()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;

            volumeMultiplier = audioSource.volume;

            audioSource.volume = AudioController.GetVolume(AudioType.Music) * volumeMultiplier;

            AudioController.VolumeChanged += OnVolumeChanged;
        }

        public void Unload()
        {
            AudioController.VolumeChanged -= OnVolumeChanged;
        }

        private void OnDestroy()
        {
            AudioController.VolumeChanged -= OnVolumeChanged;
        }

        public void SetAsDefault()
        {
            defaultMusicSource = this;
        }

        public void Activate()
        {
            if (activeMusicSource == this) return;

            if(activeMusicSource != null)
            {
                activeMusicSource.audioSource.volume = 0.0f;
                activeMusicSource.audioSource.Stop();
            }

            audioSource.Play();

            Fade(1.0f, DEFAULT_FADE_DURATION);

            activeMusicSource = this;
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume * AudioController.GetVolume(AudioType.Music) * volumeMultiplier;
        }

        public void Fade(float value, float duration, float delay = 0, SimpleCallback onComplete = null)
        {
            fadeTweenCase.KillActive();

            fadeTweenCase = Tween.DoFloat(audioSource.volume, value, duration, (value) =>
            {
                audioSource.volume = value * AudioController.GetVolume(AudioType.Music) * volumeMultiplier;
            }, delay).OnComplete(onComplete);
        }

        private void OnVolumeChanged(AudioType audioType, float volume)
        {
            if (audioType != AudioType.Music) return;

            audioSource.volume = volume;
        }

        public bool IsActive()
        {
            return activeMusicSource == this;
        }

        public static void ActivateDefault()
        {
            if (activeMusicSource == defaultMusicSource) return;

            defaultMusicSource.Activate();
        }

        private static void UnloadStatic()
        {
            defaultMusicSource = null;
            activeMusicSource = null;
        }
    }
}

// -----------------
// Audio Controller v 0.4
// -----------------

// Changelog
// v 0.4
// â€¢ Vibration settings removed
// v 0.3.3
// â€¢ Method for separate music and sound volume override
// v 0.3.2
// â€¢ Added audio listener creation method
// v 0.3.2
// â€¢ Added volume float
// â€¢ AudioSettings variable removed (now sounds, music and vibrations can be reached directly)
// v 0.3.1
// â€¢ Added OnVolumeChanged callback
// â€¢ Renamed AudioSettings to Settings
// v 0.3
// â€¢ Added IsAudioModuleEnabled method
// â€¢ Added IsVibrationModuleEnabled method
// â€¢ Removed VibrationToggleButton class
// v 0.2
// â€¢ Removed MODULE_VIBRATION
// v 0.1
// â€¢ Added basic version
// â€¢ Added support of new initialization
// â€¢ Music and Sound volume is combined