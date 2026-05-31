namespace Things
{
    [System.Serializable]
    public class AudioSave : ISaveObject
    {
        public VolumeData[] VolumeDatas;

        public void Flush()
        {
            AudioType[] audioTypes = EnumUtils.GetEnumArray<AudioType>();

            VolumeDatas = new VolumeData[audioTypes.Length];

            for (int i = 0; i < audioTypes.Length; i++)
            {
                VolumeDatas[i] = new VolumeData() { AudioType = audioTypes[i], Volume = AudioController.GetVolume(audioTypes[i]) };
            }
        }

        [System.Serializable]
        public class VolumeData
        {
            public AudioType AudioType;
            public float Volume;
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