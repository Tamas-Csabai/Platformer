using Main.EntitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.DamageSystem
{
    public class PeriodicDamager : Damager
    {

        [SerializeField] private int defaultDamage = 1;
        [SerializeField] private float damagePeriodTime = 1f;

        private Dictionary<Entity, Coroutine> _entitiesToDamage = new Dictionary<Entity, Coroutine>();

        public int Damage { get; private set; }

        private void Awake()
        {
            Damage = defaultDamage;
        }

        public override void StartDamageEntity(Entity entity)
        {
            StartPeriodicDamage(entity);
        }

        public override void DamageEntity(Entity entity)
        {
            entity.Health.Damage(Damage);
        }

        public override void StopDamageEntity(Entity entity)
        {
            StopPeriodicDamage(entity);
        }

        public void StartPeriodicDamage(Entity entity)
        {
            if (_entitiesToDamage.ContainsKey(entity))
                return;

            DamageEntity(entity);

            Coroutine periodicDamage_Routine = StartCoroutine(PeriodicDamage_Routine(entity));
            _entitiesToDamage.Add(entity, periodicDamage_Routine);
        }

        public void StopPeriodicDamage(Entity entity)
        {
            if (!_entitiesToDamage.ContainsKey(entity))
                return;

            StopCoroutine(_entitiesToDamage[entity]);
            _entitiesToDamage.Remove(entity);
        }

        private IEnumerator PeriodicDamage_Routine(Entity entity)
        {
            float time = 0;

            while (true)
            {
                if (time > damagePeriodTime)
                {
                    time = 0;
                    DamageEntity(entity);
                }

                time += Time.deltaTime;

                yield return null;
            }
        }

    }
}
