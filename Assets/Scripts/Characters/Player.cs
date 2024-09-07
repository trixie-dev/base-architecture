using EMA;
using EMA.Examples;

namespace Characters
{
    public class Player : Entity
    {
        public override void Initialize(int entityId)
        {
            base.Initialize(entityId);

            Modules
                .Add<EntityModuleExample>()
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