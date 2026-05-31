using UnityEngine;

namespace Things.Game.Configs.PlayerConfig
{
    [CreateAssetMenu(menuName = "Things/Player/Movement Config")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [Header("Speed")]
        public float BaseMaxSpeed = 5f;
        public float RunSpeedMultiplier = 1.5f;
        public float SwimmingSpeedMultiplier = 0.5f;
        
        [Header("Controls")]
        public float MovementInputThreshold = 0.1f;
        public float RotationLerpSpeed = 10f;
    }
}
