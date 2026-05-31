using System;

namespace Things.Game.Models
{
    public class MissionProgressModel
    {
        public string MissionId { get; private set; }
        public int CurrentProgress { get; private set; }
        public int TargetProgress { get; private set; }
        public bool IsCompleted => CurrentProgress >= TargetProgress;

        public event Action OnProgressChanged;
        public event Action OnCompleted;

        public void Initialize(string missionId, int targetProgress)
        {
            MissionId = missionId;
            TargetProgress = targetProgress;
            CurrentProgress = 0;
        }

        public void AddProgress(int amount)
        {
            if (IsCompleted) return;
            CurrentProgress += amount;
            OnProgressChanged?.Invoke();
            if (IsCompleted) OnCompleted?.Invoke();
        }
    }
}
