using System;
using UnityEngine.Events;

namespace EMA
{
    /// <summary>
    /// If you need to create a special component for an entity, use this interface.
    /// </summary>
    public interface IEntityModule
    {
        public IEntity Entity { get; set; }
        public bool Enabled { get; set; }
        public bool IsInitialized { get; set; }
        public UnityEvent OnInitialized { get; }

        public virtual void Initialize(IEntity entity) { }
        protected void Initialized() { }
        public virtual void Update() {}
    }
}
