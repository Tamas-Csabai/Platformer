
using Main.EntitySystem;
using Main.SFX;
using UnityEngine;

namespace Main.PlayerSystem
{
    public class Player : MonoBehaviour, ISetup
    {

        private static Player s_instance;

        public static GameObject GameObject => s_instance.gameObject;
        public static PlayerController Controller => s_instance.playerController;
        public static Entity Entity => s_instance.entity;
        public static PlayerStats Stats => s_instance.playerStats;
        public static MusicController MusicController => s_instance.musicController;

        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Entity entity;
        [SerializeField] private MusicController musicController;

        void ISetup.Setup()
        {
            if (s_instance == null)
                s_instance = this;
        }

        private void OnDestroy()
        {
            if (s_instance != null && s_instance == this)
                s_instance = null;
        }

        public static bool CheckRangeSqr(Transform targetTransform, float sqrRange)
        {
            if ((GameObject.transform.position - targetTransform.position).sqrMagnitude < sqrRange)
                return true;

            return false;
        }

        public static bool CheckRange(Transform targetTransform, float range)
        {
            if ((GameObject.transform.position - targetTransform.position).magnitude < range)
                return true;

            return false;
        }

    }
}
