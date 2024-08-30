using ECA;

namespace Characters
{
    public class PlayerInput : EntityComponent
    {
        public override void Initialize(IEntity entity)
        {
            base.Initialize(entity);
            // component initialization here
            Initialized();
        }

        public override void Update()
        {
            // component update here
        }
    }
}