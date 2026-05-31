using UnityEngine;
using Things.Core.Infrastructure;
using Things.Game.Configs.Environment;

namespace Things.Game.Environment
{
    public class EnvironmentPresenter : MonoBehaviour
    {
        private Light directionalLight;

        private void Awake()
        {
            directionalLight = FindFirstObjectByType<Light>();
        }

        public void SetPreset(EnvironmentPresetConfig preset, WeatherPresetConfig weather)
        {
            RenderSettings.skybox = preset.SkyboxMaterial;
            RenderSettings.ambientLight = preset.AmbientColor;
            RenderSettings.fogDensity = preset.FogDensity;
            RenderSettings.fogColor = preset.FogColor;

            if (directionalLight != null && weather != null)
            {
                directionalLight.intensity *= weather.DirectionalLightIntensityMultiplier;
            }

            // Spawn weather particles
            if (weather != null && weather.WeatherParticlePrefab != null)
            {
                Instantiate(weather.WeatherParticlePrefab, Camera.main.transform);
            }
        }
    }
}
