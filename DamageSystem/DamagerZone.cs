using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.EntitySystem;

namespace Main.DamageSystem
{
    public class DamagerZone : MonoBehaviour
    {

        [SerializeField] private EntityCollision entityCollision;
        [SerializeField] private PeriodicDamager periodicDamager;

        private void Awake()
        {
            entityCollision.OnEnter += OnEntityEnter;
            entityCollision.OnExit += OnEntityExit;
        }

        private void OnDestroy()
        {
            if(entityCollision != null)
            {
                entityCollision.OnEnter -= OnEntityEnter;
                entityCollision.OnExit -= OnEntityExit;
            }
        }

        private void OnEntityEnter(Entity entity)
        {
            periodicDamager.StartPeriodicDamage(entity);
        }

        private void OnEntityExit(Entity entity)
        {
            periodicDamager.StopPeriodicDamage(entity);
        }

    }
}
