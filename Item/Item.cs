using Main.EffectSystem;
using Main.EntitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.ItemSystem
{
    public class Item : MonoBehaviour
    {

        [SerializeField] private EntityCollision entityCollision;
        [SerializeField] private Effect[] effects;

        private void Awake()
        {
            entityCollision.OnEnter += Collect;
        }

        private void OnDestroy()
        {
            if(entityCollision != null)
                entityCollision.OnEnter -= Collect;
        }

        private void Collect(Entity entity)
        {

            for (int i = 0; i < effects.Length; i++)
                effects[i].Execute();

            Destroy(gameObject);
        }

    }
}
