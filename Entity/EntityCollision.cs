using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.EntitySystem
{
    public class EntityCollision : MonoBehaviour
    {

        public EntitySO[] iteractableEntities;

        public System.Action<Entity> OnEnter;
        public System.Action<Entity> OnExit;

        public void Enter(Entity entity)
        {
            if (iteractableEntities == null || iteractableEntities.HasElement(entity.EntitySO))
                OnEnter?.Invoke(entity);
        }

        public void Exit(Entity entity)
        {
            if (iteractableEntities == null || iteractableEntities.HasElement(entity.EntitySO))
                OnExit?.Invoke(entity);
        }

    }
}
