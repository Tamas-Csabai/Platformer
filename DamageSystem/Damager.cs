using Main.EntitySystem;
using UnityEngine;

namespace Main.DamageSystem
{
    public abstract class Damager : MonoBehaviour
    {

        public virtual void StartDamageEntity(Entity entity)
        {
            DamageEntity(entity);
        }

        public abstract void DamageEntity(Entity entity);

        public virtual void StopDamageEntity(Entity entity)
        {

        }
    }
}
