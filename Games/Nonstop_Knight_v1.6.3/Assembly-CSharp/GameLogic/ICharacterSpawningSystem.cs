namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;

    public interface ICharacterSpawningSystem
    {
        int getPreviousMobSpawnpointIndex();
        void refreshPetSummons(Player player);
        void spawnRoomMinionHordeAtSpawnpoint(Room room, Room.Spawnpoint spawnPt, int spawnCount, [Optional, DefaultParameterValue(null)] string fixedCharacterId);
        void summonActiveRoomBoss();
        void summonSupportCritters(CharacterInstance owningCharacter, int critterCount, [Optional, DefaultParameterValue(null)] Vector3? fixedSpawnPos);
        void trySummonWildBoss();
    }
}

