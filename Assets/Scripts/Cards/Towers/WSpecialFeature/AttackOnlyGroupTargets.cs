﻿using UnityEngine;

namespace Assets.Scripts.Cards.Towers.WSpecialFeature
{
    [CreateAssetMenu(menuName = "Card/WSpecialFeature/AttackOnlyGroupTargets")]
    public class AttackOnlyGroupTargets : Card
    {
        public int DefaultTargetsNumber;
        public override System.Action GetTowerSetUpOnSpell(BulletTower tower, GameTile tile = null)
        {
            tower.SetDefaultTargetsNumber(DefaultTargetsNumber);
            tower.ActivateOnlyGroupTargets();
            return null;
        }
    }
}
