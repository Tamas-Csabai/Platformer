using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.EntitySystem
{
    public class EntityCollider : MonoBehaviour
    {

        [SerializeField] private Entity entity;

        private void OnTriggerEnter(Collider other)
        {
            EntityCollision entityCollision;

            if (other.attachedRigidbody != null)
            {
                if (!other.attachedRigidbody.TryGetComponent(out entityCollision))
                    return;
            }
            else
            {
                if (!other.TryGetComponent(out entityCollision))
                    return;
            }

            entityCollision.Enter(entity);
        }

        private void OnTriggerExit(Collider other)
        {
            EntityCollision entityCollision;

            if (other.attachedRigidbody != null)
            {
                if (!other.attachedRigidbody.TryGetComponent(out entityCollision))
                    return;
            }
            else
            {
                if (!other.TryGetComponent(out entityCollision))
                    return;
            }

            entityCollision.Exit(entity);
        }

    }
}
