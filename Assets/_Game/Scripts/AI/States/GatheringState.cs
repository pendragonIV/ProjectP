using UnityEngine;

namespace Things.Game.AI.States
{
    public class GatheringState : HelperStateBase
    {
        public GatheringState(HelperPresenter presenter) : base(presenter) { }

        public override void Enter()
        {
            // Tìm cây gần nhất
        }

        public override void Update(float deltaTime)
        {
            // Di chuyển tới cây và chặt
        }
    }
}
