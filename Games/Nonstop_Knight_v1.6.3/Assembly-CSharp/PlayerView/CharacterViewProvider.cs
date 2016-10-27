namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CharacterViewProvider : ITypedInstanceProvider<CharacterView, CharacterPrefab>
    {
        private Dictionary<CharacterPrefab, GameObjectProvider> m_gops;
        private int m_layer;
        private Transform m_objectPoolParentTm;

        public CharacterViewProvider(int layer, Transform objectPoolParentTm)
        {
            this.m_layer = layer;
            this.m_objectPoolParentTm = objectPoolParentTm;
            this.m_gops = new Dictionary<CharacterPrefab, GameObjectProvider>(new CharacterPrefabBoxAvoidanceComparer());
        }

        public CharacterView instantiate(CharacterPrefab characterPrefab)
        {
            if (!this.m_gops.ContainsKey(characterPrefab))
            {
                this.m_gops.Add(characterPrefab, new GameObjectProvider("Prefabs/Characters/" + characterPrefab, this.m_objectPoolParentTm, this.m_layer));
            }
            GameObject obj2 = this.m_gops[characterPrefab].instantiate();
            CharacterView characterView = obj2.AddComponent<CharacterView>();
            switch (characterPrefab)
            {
                case CharacterPrefab.KnightMale:
                case CharacterPrefab.KnightFemale:
                case CharacterPrefab.KnightClone:
                    characterView.Animator = obj2.AddComponent<HeroCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<HeroCharacterAudio>();
                    break;

                case CharacterPrefab.Critter:
                    characterView.Animator = obj2.AddComponent<CritterCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<CritterCharacterAudio>();
                    break;

                case CharacterPrefab.Skeleton:
                case CharacterPrefab.SkeletonBuckler:
                case CharacterPrefab.SkeletonMetal:
                case CharacterPrefab.SkeletonMitts:
                case CharacterPrefab.SkeletonPunk:
                    characterView.Animator = obj2.AddComponent<SkeletonMeleeCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<SkeletonCharacterAudio>();
                    break;

                case CharacterPrefab.SkeletonCaster:
                    characterView.Animator = obj2.AddComponent<SkeletonCasterCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<SkeletonCharacterAudio>();
                    break;

                case CharacterPrefab.SkeletonCasterRed:
                    characterView.Animator = obj2.AddComponent<SkeletonCasterRedCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<SkeletonCharacterAudio>();
                    break;

                case CharacterPrefab.PygmyMelee:
                case CharacterPrefab.PygmyRanged:
                case CharacterPrefab.PygmyCaster:
                case CharacterPrefab.PygmySummoner:
                    characterView.Animator = obj2.AddComponent<PygmyCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<PygmyCharacterAudio>();
                    break;

                case CharacterPrefab.JellyGreen:
                case CharacterPrefab.JellyOrange:
                case CharacterPrefab.JellyPurple:
                case CharacterPrefab.JellySpiked:
                    characterView.Animator = obj2.AddComponent<JellyBasicCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<JellyCharacterAudio>();
                    break;

                case CharacterPrefab.PetDragon1:
                case CharacterPrefab.PetDragon2:
                case CharacterPrefab.PetShark1:
                    characterView.Animator = obj2.AddComponent<DragonCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<DragonCharacterAudio>();
                    break;

                case CharacterPrefab.WorgBrown1:
                case CharacterPrefab.WorgBrown2:
                case CharacterPrefab.WorgBlue1:
                case CharacterPrefab.WorgBlue2:
                    characterView.Animator = obj2.AddComponent<WorgCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<WorgCharacterAudio>();
                    break;

                case CharacterPrefab.CrocodileMelee1:
                    characterView.Animator = obj2.AddComponent<CrocodileFishCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<CrocodileCharacterAudio>();
                    break;

                case CharacterPrefab.CrocodileMelee2:
                    characterView.Animator = obj2.AddComponent<CrocodileSpearCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<CrocodileCharacterAudio>();
                    break;

                case CharacterPrefab.CrocodileChieftain:
                    characterView.Animator = obj2.AddComponent<CrocodileChieftainCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<CrocodileCharacterAudio>();
                    break;

                case CharacterPrefab.RatMelee1:
                case CharacterPrefab.RatMelee2:
                case CharacterPrefab.RatMelee3:
                case CharacterPrefab.RatSummoner1:
                    characterView.Animator = obj2.AddComponent<RatCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<RatCharacterAudio>();
                    break;

                case CharacterPrefab.AnubisGuard:
                    characterView.Animator = obj2.AddComponent<AnubisGuardCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<AnubisCharacterAudio>();
                    break;

                case CharacterPrefab.AnubisMelee1:
                case CharacterPrefab.AnubisMelee2:
                    characterView.Animator = obj2.AddComponent<AnubisMeleeCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<AnubisCharacterAudio>();
                    break;

                case CharacterPrefab.MummyMelee:
                    characterView.Animator = obj2.AddComponent<MummyMeleeCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<MummyCharacterAudio>();
                    break;

                case CharacterPrefab.MummyRanged:
                    characterView.Animator = obj2.AddComponent<MummyRangedCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<MummyCharacterAudio>();
                    break;

                case CharacterPrefab.GoblinFat1:
                case CharacterPrefab.GoblinFat2:
                case CharacterPrefab.GoblinSkinny1:
                case CharacterPrefab.GoblinSkinny2:
                    characterView.Animator = obj2.AddComponent<GoblinCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<GoblinCharacterAudio>();
                    break;

                case CharacterPrefab.YetiMelee1:
                case CharacterPrefab.YetiRanged1:
                    characterView.Animator = obj2.AddComponent<YetiCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<YetiCharacterAudio>();
                    break;

                case CharacterPrefab.ShroomMelee1:
                case CharacterPrefab.ShroomMelee2:
                    characterView.Animator = obj2.AddComponent<ShroomMeleeCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<ShroomCharacterAudio>();
                    break;

                case CharacterPrefab.ShroomPuffer1:
                case CharacterPrefab.ShroomPuffer2:
                    characterView.Animator = obj2.AddComponent<ShroomPufferCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<ShroomCharacterAudio>();
                    break;

                case CharacterPrefab.PetCrab1:
                case CharacterPrefab.PetCrab2:
                    characterView.Animator = obj2.AddComponent<CrabCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<CrabCharacterAudio>();
                    break;

                case CharacterPrefab.PetSquid1:
                case CharacterPrefab.PetSquid2:
                    characterView.Animator = obj2.AddComponent<SquidCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<SquidCharacterAudio>();
                    break;

                case CharacterPrefab.PetYeti1:
                    characterView.Animator = obj2.AddComponent<PetYetiCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<GorillaCharacterAudio>();
                    break;

                case CharacterPrefab.PetWalrus1:
                    characterView.Animator = obj2.AddComponent<WalrusCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<WalrusCharacterAudio>();
                    break;

                case CharacterPrefab.PetShark2:
                    characterView.Animator = obj2.AddComponent<SchoolOfSharksCharacterAnimator>();
                    break;

                case CharacterPrefab.PetStumpy1:
                    characterView.Animator = obj2.AddComponent<StumpyGroundCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<StumpyCharacterAudio>();
                    break;

                case CharacterPrefab.PetStumpy2:
                    characterView.Animator = obj2.AddComponent<StumpyFlyCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<StumpyCharacterAudio>();
                    break;

                case CharacterPrefab.PetDog1:
                    characterView.Animator = obj2.AddComponent<DogCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<DogCharacterAudio>();
                    break;

                case CharacterPrefab.PetMoose1:
                    characterView.Animator = obj2.AddComponent<MooseCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<MooseCharacterAudio>();
                    break;

                case CharacterPrefab.PetRam1:
                    characterView.Audio = obj2.AddComponent<RamCharacterAudio>();
                    characterView.Animator = obj2.AddComponent<RamCharacterAnimator>();
                    break;

                case CharacterPrefab.PetBeaver1:
                    characterView.Animator = obj2.AddComponent<BeaverCharacterAnimator>();
                    break;

                case CharacterPrefab.PetChest1:
                    characterView.Animator = obj2.AddComponent<ChestCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<ChestCharacterAudio>();
                    break;

                case CharacterPrefab.PetPanda1:
                    characterView.Animator = obj2.AddComponent<PandaCharacterAnimator>();
                    characterView.Audio = obj2.AddComponent<PandaCharacterAudio>();
                    break;
            }
            if (characterView.Animator != null)
            {
                characterView.Animator.initialize(characterView);
            }
            if (characterView.Audio != null)
            {
                characterView.Audio.initialize(characterView);
            }
            characterView.postAwake(characterPrefab);
            obj2.SetActive(false);
            return characterView;
        }

        public void onDestroy(CharacterView obj)
        {
            this.m_gops[obj.CharacterPrefab].onDestroy(obj.gameObject);
        }

        public void onReturn(CharacterView obj)
        {
            if (obj.Audio != null)
            {
                obj.Audio.cleanup();
            }
            if (obj.Animator != null)
            {
                obj.Animator.cleanup();
            }
            this.m_gops[obj.CharacterPrefab].onReturn(obj.gameObject);
        }

        public void reset()
        {
            foreach (KeyValuePair<CharacterPrefab, GameObjectProvider> pair in this.m_gops)
            {
                pair.Value.onReset();
            }
        }
    }
}

