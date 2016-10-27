namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using UnityEngine;

    public class UiShowroom : MonoBehaviour
    {
        protected void Awake()
        {
            App.Binder.LocaSystem = new LocaSystem();
            App.Binder.PersistentObjectRootTm = new GameObject("PersistentObjectRoot").transform;
            Service.Binder.EventBus = new Service.EventBus();
            Service.Binder.ContentService = new ContentService();
            Service.Binder.ContentService.InitializeContent();
            GameLogic.Binder.GameState = new GameState();
            GameLogic.Binder.GameState.Player = new Player();
            GameLogic.Binder.EventBus = new GameLogic.EventBus();
            GameLogic.Binder.CharacterResources = new CharacterResources();
            PlayerView.Binder.DisabledUiMaterial = Resources.Load<Material>("Materials/ui_disabled");
            PlayerView.Binder.MonochromeUiMaterial = Resources.Load<Material>("Materials/ui_monochrome_boosted");
            PlayerView.Binder.MaterialStorage = new MaterialStorage();
            PlayerView.Binder.EventBus = new PlayerView.EventBus();
            GameObject obj2 = new GameObject("AssetBundleLoader");
            obj2.transform.SetParent(base.transform);
            App.Binder.AssetBundleLoader = obj2.AddComponent<AssetBundleLoader>();
            GameObject obj3 = ResourceUtil.Instantiate<GameObject>("Prefabs/InputSystem");
            obj3.transform.SetParent(base.transform);
            PlayerView.Binder.InputSystem = obj3.GetComponent<InputSystem>();
            PlayerView.Binder.SpriteResources = new SpriteResources();
            GameLogic.Binder.ItemResources = new ItemResources();
            PlayerView.Binder.PersistentAudioSourcePool = new TypedObjectPool<PoolableAudioSource, AudioSourceType>(new PoolableAudioSourceProvider(Layers.DEFAULT, App.Binder.PersistentObjectRootTm), 1, ConfigObjectPools.PERSISTENT_AUDIO_SOURCES, ObjectPoolExpansionMethod.DOUBLE, true);
            GameObject obj4 = new GameObject("AudioSystem");
            obj4.transform.SetParent(base.transform);
            PlayerView.Binder.AudioSystem = obj4.AddComponent<AudioSystem>();
            PlayerView.Binder.PersistentParticleEffectPool = new TypedObjectPool<PoolableParticleSystem, EffectType>(new PoolableParticleSystemProvider(App.Binder.PersistentObjectRootTm), 1, ConfigObjectPools.PERSISTENT_PARTICLE_EFFECTS, ObjectPoolExpansionMethod.DOUBLE, true);
            GameObject obj5 = new GameObject("EffectSystem");
            obj5.transform.SetParent(base.transform);
            PlayerView.Binder.EffectSystem = obj5.AddComponent<EffectSystem>();
            PlayerView.Binder.RewardGalleryCellPool = new TypedObjectPool<RewardGalleryCell, RewardGalleryCellType>(new RewardGalleryCellProvider(App.Binder.PersistentObjectRootTm), 8, null, ObjectPoolExpansionMethod.DOUBLE, true);
            PlayerView.Binder.MenuTreasureChestPool = new ObjectPool<MenuTreasureChest>(new MenuTreasureChestProvider("Prefabs/Menu/MenuTreasureChest", Layers.UI), 8, ObjectPoolExpansionMethod.DOUBLE, true);
            PlayerView.Binder.ItemCellPool = new ObjectPool<ItemCell>(new ItemCellProvider("Prefabs/Menu/ItemCell", Layers.DEFAULT), 0x20, ObjectPoolExpansionMethod.DOUBLE, true);
            GameObject obj6 = new GameObject("MenuSystem");
            obj6.transform.SetParent(base.transform);
            PlayerView.Binder.MenuSystem = obj6.AddComponent<MenuSystem>();
            PlayerView.Binder.MenuSystem.MenuCamera.enabled = true;
            PlayerView.Binder.MenuSystem.MenuCamera.orthographicSize = 1104f;
            GameObject obj7 = new GameObject("AdsSystem");
            obj7.transform.SetParent(base.transform);
            Service.Binder.AdsSystem = obj7.AddComponent<AdsSystem>();
        }
    }
}

