using Main.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.EffectSystem
{
    public class AddCoinEffect : Effect
    {

        [SerializeField] private int defaultAddCoinAmount = 1;

        public override void Execute()
        {
            Player.Stats.CurrentCoinAmount.Value += defaultAddCoinAmount;
        }
    }
}
