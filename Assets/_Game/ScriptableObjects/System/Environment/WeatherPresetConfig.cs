using UnityEngine;

namespace Things.Game.Configs.Environment
{
    [CreateAssetMenu(menuName = "Things/Environment/Weather Config")]
    public class WeatherPresetConfig : ScriptableObject
    {
        public float DirectionalLightIntensityMultiplier = 1f;
        public GameObject WeatherParticlePrefab;
    }
}
