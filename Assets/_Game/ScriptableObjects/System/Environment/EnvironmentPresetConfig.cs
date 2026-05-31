using UnityEngine;

namespace Things.Game.Configs.Environment
{
    [CreateAssetMenu(menuName = "Things/Environment/Preset Config")]
    public class EnvironmentPresetConfig : ScriptableObject
    {
        public Material SkyboxMaterial;
        public Color AmbientColor = Color.white;
        public float FogDensity = 0.01f;
        public Color FogColor = Color.gray;
    }
}
