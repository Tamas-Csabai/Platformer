using Main.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.EntitySystem
{
    public class Entity : MonoBehaviour
    {

        [SerializeField] private EntitySO entitySO;
        [SerializeField] private Health health;

        public EntitySO EntitySO => entitySO;
        public Health Health => health;

        private void Awake()
        {
            health.OnDie += Destroy;
        }

        private void OnDestroy()
        {
            if(health != null)
                health.OnDie -= Destroy;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

    }
}
