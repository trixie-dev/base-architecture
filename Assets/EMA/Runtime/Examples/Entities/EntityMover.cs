using UnityEngine;

namespace EMA.Examples
{
    public class EntityMover : EntityModule
    {
        private float _moveSpeed;
        public Vector2 _desiredDirection;

        public override void Initialize(IEntity entity)
        {
            base.Initialize(entity);
            // component initialization here
            Initialized();
        }

        public override void Update()
        {
            // component update here
            Entity.Transform.Translate(_desiredDirection * (_moveSpeed * Time.deltaTime));
        }

        public void SetDirection(Vector2 direction)
        {
            _desiredDirection = direction;
        }
        public void SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }


    }
    
    public class EntityStats : EntityModule
    {
        public float MoveSpeed { get; set; } = 5f;
        public float Health { get; set; } = 100f;
        
        public override void Initialize(IEntity entity)
        {
            base.Initialize(entity);
            // component initialization here
            Initialized();
        }

        public override void Update()
        {
        }
    }
    
    public class EngineModule : EntityModule
    {
        private EntityStats _stats;
        
        public float MoveSpeedMultiplier { get; set; } = 1f;
        
        public delegate void OnMoveSpeedChanged(float moveSpeed);
        public OnMoveSpeedChanged OnMoveSpeedChangedEvent;
        
        
        public override void Initialize(IEntity entity)
        {
            base.Initialize(entity);
            _stats = Entity.GetModule<EntityStats>();
            Initialized();
        }

        public override void Update()
        {
           MoveSpeedMultiplier += 0.1f * Time.deltaTime;
           OnMoveSpeedChangedEvent?.Invoke(GetMoveSpeed());
        }

        public float GetMoveSpeed()
        {
            return _stats.MoveSpeed * MoveSpeedMultiplier;
        }
    }
}