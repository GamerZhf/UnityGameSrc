namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MaterialStorage
    {
        private Dictionary<Material, Material> m_sharedGenericMaterialInstances = new Dictionary<Material, Material>();
        private Dictionary<CharacterPrefab, Material> m_sharedKnockedOutMaterialInstancesGameplay;
        private Dictionary<CharacterPrefab, Material> m_sharedKnockedOutMaterialInstancesMenu;
        private Dictionary<Material, Material> m_sharedMenuMaterialInstances = new Dictionary<Material, Material>();
        private Dictionary<Material, Material> m_sharedTransparentMaterialInstances = new Dictionary<Material, Material>();
        public const string SHARED_MATERIAL_POSTFIX_TAG = "-SHARED_INSTANCE";

        public Material getSharedGenericMaterial(Material refMaterial)
        {
            if (refMaterial == null)
            {
                return null;
            }
            if (refMaterial.name.EndsWith("-SHARED_INSTANCE"))
            {
                return refMaterial;
            }
            if (this.m_sharedGenericMaterialInstances.ContainsKey(refMaterial))
            {
                return this.m_sharedGenericMaterialInstances[refMaterial];
            }
            Material material = new Material(refMaterial);
            material.name = material.name + "-SHARED_INSTANCE";
            this.m_sharedGenericMaterialInstances.Add(refMaterial, material);
            return material;
        }

        public Material getSharedKnockedOutMaterialForCharacterPrefab(CharacterPrefab characterPrefab, bool menuVersion)
        {
            if ((this.m_sharedKnockedOutMaterialInstancesGameplay == null) || (this.m_sharedKnockedOutMaterialInstancesMenu == null))
            {
                List<KeyValuePair<CharacterPrefab, string>> list2 = new List<KeyValuePair<CharacterPrefab, string>>();
                list2.Add(new KeyValuePair<CharacterPrefab, string>(CharacterPrefab.KnightMale, "Materials/ko_knight_male"));
                list2.Add(new KeyValuePair<CharacterPrefab, string>(CharacterPrefab.KnightFemale, "Materials/ko_knight_female"));
                List<KeyValuePair<CharacterPrefab, string>> list = list2;
                this.m_sharedKnockedOutMaterialInstancesGameplay = new Dictionary<CharacterPrefab, Material>(new CharacterPrefabBoxAvoidanceComparer());
                this.m_sharedKnockedOutMaterialInstancesMenu = new Dictionary<CharacterPrefab, Material>(new CharacterPrefabBoxAvoidanceComparer());
                for (int i = 0; i < list.Count; i++)
                {
                    KeyValuePair<CharacterPrefab, string> pair = list[i];
                    Material material = ResourceUtil.Instantiate<Material>(pair.Value);
                    KeyValuePair<CharacterPrefab, string> pair2 = list[i];
                    this.m_sharedKnockedOutMaterialInstancesGameplay.Add(pair2.Key, material);
                    Material material2 = new Material(Shader.Find("CUSTOM/Menu_Character_DiffuseWrap"));
                    material2.CopyPropertiesFromMaterial(PlayerView.Binder.MenuSystem.ReferenceMenuLightingMaterial);
                    material2.mainTexture = material.mainTexture;
                    KeyValuePair<CharacterPrefab, string> pair3 = list[i];
                    this.m_sharedKnockedOutMaterialInstancesMenu.Add(pair3.Key, material2);
                }
            }
            switch (characterPrefab)
            {
                case CharacterPrefab.KnightMale:
                case CharacterPrefab.KnightFemale:
                    if (menuVersion)
                    {
                        return this.m_sharedKnockedOutMaterialInstancesMenu[characterPrefab];
                    }
                    return this.m_sharedKnockedOutMaterialInstancesGameplay[characterPrefab];
            }
            Debug.LogError("No knocked out material defined for prefab: " + characterPrefab);
            return null;
        }

        public Material getSharedMenuMaterial(Material gameplayMaterial)
        {
            if (gameplayMaterial == null)
            {
                return null;
            }
            if (gameplayMaterial.name.EndsWith("-SHARED_INSTANCE"))
            {
                return gameplayMaterial;
            }
            if (this.m_sharedMenuMaterialInstances.ContainsKey(gameplayMaterial))
            {
                return this.m_sharedMenuMaterialInstances[gameplayMaterial];
            }
            Material material = new Material(gameplayMaterial);
            material.name = material.name + "-SHARED_INSTANCE";
            Texture mainTexture = material.mainTexture;
            material.CopyPropertiesFromMaterial(PlayerView.Binder.MenuSystem.ReferenceMenuLightingMaterial);
            material.mainTexture = mainTexture;
            if (gameplayMaterial.shader.name == "CUSTOM/DropShadow")
            {
                material.shader = Shader.Find("CUSTOM/Transparent_MaterialColor");
                material.SetColor("_Color", new Color(1f, 1f, 1f, 0.627451f));
            }
            else if (gameplayMaterial.shader.name == "CUSTOM/Character")
            {
                material.shader = Shader.Find("CUSTOM/Menu_Character_DiffuseWrap");
            }
            else if (gameplayMaterial.shader.name == "CUSTOM/Helmet")
            {
                material.shader = Shader.Find("CUSTOM/Menu_Helmet_DiffuseWrap");
            }
            else if (gameplayMaterial.shader.name == "CUSTOM/Particles_Additive")
            {
                material.shader = Shader.Find("CUSTOM/Particles_Invisible");
            }
            else
            {
                material.shader = Shader.Find("CUSTOM/Menu_Character_DiffuseWrap_Backfaces");
            }
            this.m_sharedMenuMaterialInstances.Add(gameplayMaterial, material);
            return material;
        }

        public Material getSharedTransparentMaterial(Material opaqueMaterial)
        {
            if (this.m_sharedTransparentMaterialInstances.ContainsKey(opaqueMaterial))
            {
                return this.m_sharedTransparentMaterialInstances[opaqueMaterial];
            }
            Material material = this.instantiateTransparentMaterial(opaqueMaterial);
            material.name = material.name + "-SHARED";
            this.m_sharedTransparentMaterialInstances.Add(opaqueMaterial, material);
            return material;
        }

        public Material instantiateTransparentMaterial(Material opaqueMaterial)
        {
            Material material = new Material(opaqueMaterial);
            material.name = material.name + "-TRANSPARENT_INSTANCE";
            Texture mainTexture = material.mainTexture;
            material.mainTexture = mainTexture;
            if (((opaqueMaterial.shader.name == "CUSTOM/Deco") || (opaqueMaterial.shader.name == "CUSTOM/Deco_Tournament")) || (opaqueMaterial.shader.name == "CUSTOM/Deco_Tournament_Cloth"))
            {
                material.shader = Shader.Find("CUSTOM/Deco_Transparent");
                return material;
            }
            if (opaqueMaterial.shader.name == "CUSTOM/Deco_Bright")
            {
                material.shader = Shader.Find("CUSTOM/Deco_Transparent_Bright");
                return material;
            }
            if (!(opaqueMaterial.shader.name == "CUSTOM/Deco_Prop") && (opaqueMaterial.shader.name == "CUSTOM/Deco_Prop_Opaque"))
            {
            }
            return material;
        }

        public void releaseSharedGenericMaterialReference(Material sharedMaterial)
        {
            if (this.m_sharedGenericMaterialInstances.ContainsKey(sharedMaterial))
            {
                this.m_sharedGenericMaterialInstances.Remove(sharedMaterial);
            }
        }
    }
}

