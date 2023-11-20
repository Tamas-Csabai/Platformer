using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.PlayerSystem
{
    public class PlayerStats : MonoBehaviour
    {

        [field: SerializeField] public Stat<int> CurrentCoinAmount { get; private set; }

        private void Start()
        {
            CurrentCoinAmount.Reset();
        }

    }
}
