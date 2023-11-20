
using UnityEngine;

namespace Main.DamageSystem
{
    public class Health : MonoBehaviour
    {
        public System.Action<int> OnDamage;
        public System.Action OnDie;

        [field:SerializeField] public Stat<int> CurrentHealth { get; protected set; }

        protected virtual void Start()
        {
            CurrentHealth.Reset();
        }

        public virtual void Damage(int damage)
        {
            CurrentHealth.Value -= damage;

            if (CurrentHealth.Value <= 0)
                Die();

            OnDamage?.Invoke(damage);
        }

        public virtual void Kill()
        {
            Die();
        }

        protected virtual void Die()
        {
            CurrentHealth.Value = 0;

            OnDie?.Invoke();
        }

    }
}
