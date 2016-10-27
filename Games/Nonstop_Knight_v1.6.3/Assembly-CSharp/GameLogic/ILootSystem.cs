namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface ILootSystem
    {
        void awardBossRewards(ActiveDungeon ad, GameLogic.CharacterType killedBossType, bool wildBoss);
        bool grantRetirementTriggerChestIfAllowed();
        void triggerResourceExplosion(ResourceType resourceType, Vector3 centerWorldPos, double amountPerDrop, int dropCount, [Optional, DefaultParameterValue("")] string trackingId);
    }
}

