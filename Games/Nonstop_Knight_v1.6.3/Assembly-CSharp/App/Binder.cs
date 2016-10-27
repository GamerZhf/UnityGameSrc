namespace App
{
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Binder
    {
        [CompilerGenerated]
        private static Service.AppboyIOSBridge <AppboyIOSBridge>k__BackingField;
        [CompilerGenerated]
        private static App.AppContext <AppContext>k__BackingField;
        [CompilerGenerated]
        private static IAssetBundleLoader <AssetBundleLoader>k__BackingField;
        [CompilerGenerated]
        private static App.BuildResources <BuildResources>k__BackingField;
        [CompilerGenerated]
        private static Transform <DynamicObjectRootTm>k__BackingField;
        [CompilerGenerated]
        private static App.IEventBus <EventBus>k__BackingField;
        [CompilerGenerated]
        private static App.LocalNotificationSystem <LocalNotificationSystem>k__BackingField;
        [CompilerGenerated]
        private static LocaSystem <LocaSystem>k__BackingField;
        [CompilerGenerated]
        private static App.NotificationRegister <NotificationRegister>k__BackingField;
        [CompilerGenerated]
        private static Transform <PersistentObjectRootTm>k__BackingField;
        [CompilerGenerated]
        private static ISocialSystem <SocialSystem>k__BackingField;
        private static MasterRemoteContent sm_editorMasterRemoteContent;

        private static void InitEditorMasterRemoteContent()
        {
            if (sm_editorMasterRemoteContent == null)
            {
                sm_editorMasterRemoteContent = ConfigApp.LoadDefaultMasterRemoteContent();
            }
        }

        public static Service.AppboyIOSBridge AppboyIOSBridge
        {
            [CompilerGenerated]
            get
            {
                return <AppboyIOSBridge>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AppboyIOSBridge>k__BackingField = value;
            }
        }

        public static App.AppContext AppContext
        {
            [CompilerGenerated]
            get
            {
                return <AppContext>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AppContext>k__BackingField = value;
            }
        }

        public static IAssetBundleLoader AssetBundleLoader
        {
            [CompilerGenerated]
            get
            {
                return <AssetBundleLoader>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AssetBundleLoader>k__BackingField = value;
            }
        }

        public static App.BuildResources BuildResources
        {
            [CompilerGenerated]
            get
            {
                return <BuildResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BuildResources>k__BackingField = value;
            }
        }

        public static App.ConfigLootTables ConfigLootTables
        {
            get
            {
                if (!Application.isPlaying)
                {
                    InitEditorMasterRemoteContent();
                    return sm_editorMasterRemoteContent.ConfigLootTables;
                }
                return Service.Binder.ContentService.MasterRemoteContent.ConfigLootTables;
            }
        }

        public static App.ConfigMeta ConfigMeta
        {
            get
            {
                if (!Application.isPlaying)
                {
                    InitEditorMasterRemoteContent();
                    return sm_editorMasterRemoteContent.ConfigMeta;
                }
                return Service.Binder.ContentService.MasterRemoteContent.ConfigMeta;
            }
        }

        public static Transform DynamicObjectRootTm
        {
            [CompilerGenerated]
            get
            {
                return <DynamicObjectRootTm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DynamicObjectRootTm>k__BackingField = value;
            }
        }

        public static App.IEventBus EventBus
        {
            [CompilerGenerated]
            get
            {
                return <EventBus>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <EventBus>k__BackingField = value;
            }
        }

        public static App.LocalNotificationSystem LocalNotificationSystem
        {
            [CompilerGenerated]
            get
            {
                return <LocalNotificationSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LocalNotificationSystem>k__BackingField = value;
            }
        }

        public static LocaSystem LocaSystem
        {
            [CompilerGenerated]
            get
            {
                return <LocaSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LocaSystem>k__BackingField = value;
            }
        }

        public static App.NotificationRegister NotificationRegister
        {
            [CompilerGenerated]
            get
            {
                return <NotificationRegister>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <NotificationRegister>k__BackingField = value;
            }
        }

        public static Transform PersistentObjectRootTm
        {
            [CompilerGenerated]
            get
            {
                return <PersistentObjectRootTm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PersistentObjectRootTm>k__BackingField = value;
            }
        }

        public static ISocialSystem SocialSystem
        {
            [CompilerGenerated]
            get
            {
                return <SocialSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SocialSystem>k__BackingField = value;
            }
        }
    }
}

