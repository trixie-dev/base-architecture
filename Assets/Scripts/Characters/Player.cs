using ECA;
using ECA.Examples;

namespace Characters
{
    public class Player : Entity
    {
        public override void Initialize(int entityId)
        {
            base.Initialize(entityId);

            Components
                .Add<EntityComponentExample>()
                .Initialize(this);

            // events 

        }

        public override void Update()
        {
            base.Update();
            // additional update logic
        }

        public override void SafeDestroy()
        {
            // additional destroy logic

            // ------------------------
            base.SafeDestroy();
        }
    }
}