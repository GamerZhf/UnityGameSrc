namespace App
{
    using System;
    using UnityEngine;

    public static class Layers
    {
        public static int AllCharactersLayerMask = -1;
        public static int AREA_EFFECTS;
        public static int AstarLayerMask = -1;
        public static int CAMERA_OCCLUDERS;
        public static int CameraOcclusionLayerMask = -1;
        public static int CHARACTER_VIEWS;
        public static int CHARACTERS;
        public static int CLICK_COLLIDERS;
        public static int DEFAULT;
        public static int DestructibleLayerMask = -1;
        public static int DUNGEON_BOOST_VIEWS;
        public static int DUNGEON_BOOSTS;
        public static int DungeonBoostEmptySpotLayerMask = -1;
        public static int EmptySpotLayerMask = -1;
        public static int GROUND;
        public static int GroundLayerMask = -1;
        public static int IGNORE_CHARACTERS;
        public static int INVISIBLE;
        public static int LineOfSightLayerMask = -1;
        public static int ObstacleLayerMask = -1;
        public static int OBSTACLES;
        public static int PARALLAX;
        public static int PROJECTILES;
        public static int SUPPORT_CHARACTERS;
        public static int TRANSPARENT_FX;
        public static int UI;

        public static void Initialize()
        {
            DEFAULT = LayerMask.NameToLayer("Default");
            TRANSPARENT_FX = LayerMask.NameToLayer("TransparentFX");
            CHARACTERS = LayerMask.NameToLayer("Characters");
            CHARACTER_VIEWS = LayerMask.NameToLayer("CharacterViews");
            IGNORE_CHARACTERS = LayerMask.NameToLayer("IgnoreCharacters");
            CLICK_COLLIDERS = LayerMask.NameToLayer("ClickColliders");
            GROUND = LayerMask.NameToLayer("Ground");
            PROJECTILES = LayerMask.NameToLayer("Projectiles");
            AREA_EFFECTS = LayerMask.NameToLayer("AreaEffects");
            OBSTACLES = LayerMask.NameToLayer("Obstacles");
            INVISIBLE = LayerMask.NameToLayer("Invisible");
            CAMERA_OCCLUDERS = LayerMask.NameToLayer("CameraOccluders");
            UI = LayerMask.NameToLayer("UI");
            PARALLAX = LayerMask.NameToLayer("Parallax");
            SUPPORT_CHARACTERS = LayerMask.NameToLayer("SupportCharacters");
            DUNGEON_BOOSTS = LayerMask.NameToLayer("DungeonBoosts");
            DUNGEON_BOOST_VIEWS = LayerMask.NameToLayer("DungeonBoostViews");
            AllCharactersLayerMask = ((((int) 1) << CHARACTERS) | (((int) 1) << IGNORE_CHARACTERS)) | (((int) 1) << SUPPORT_CHARACTERS);
            EmptySpotLayerMask = AllCharactersLayerMask | (((int) 1) << OBSTACLES);
            LineOfSightLayerMask = ((((int) 1) << CHARACTERS) | (((int) 1) << IGNORE_CHARACTERS)) | (((int) 1) << OBSTACLES);
            DungeonBoostEmptySpotLayerMask = (((int) 1) << DUNGEON_BOOSTS) | (((int) 1) << OBSTACLES);
            GroundLayerMask = ((int) 1) << GROUND;
            ObstacleLayerMask = ((int) 1) << OBSTACLES;
            CameraOcclusionLayerMask = (((int) 1) << CAMERA_OCCLUDERS) | (((int) 1) << OBSTACLES);
            AstarLayerMask = ((((int) 1) << GROUND) | (((int) 1) << CHARACTERS)) | (((int) 1) << OBSTACLES);
            DestructibleLayerMask = ((int) 1) << DUNGEON_BOOSTS;
        }
    }
}

