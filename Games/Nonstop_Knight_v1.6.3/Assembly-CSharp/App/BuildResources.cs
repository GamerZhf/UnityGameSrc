namespace App
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BuildResources
    {
        [CompilerGenerated]
        private Dictionary<string, object> <CloudBuildManifest>k__BackingField;

        public BuildResources()
        {
            TextAsset asset = ResourceUtil.LoadSafe<TextAsset>("UnityCloudBuildManifest.json", false);
            this.CloudBuildManifest = JsonUtils.Deserialize<Dictionary<string, object>>(asset.text, true);
        }

        public string getBuildInfoDescription()
        {
            string str = "Version: " + ConfigApp.BundleVersion + "-" + this.getPrettyCommitId();
            if (ConfigApp.ProductionBuild)
            {
                return str;
            }
            if (ConfigApp.IsStableBuild())
            {
                return (str + "-" + "<color=red>STABLE</color>");
            }
            return (str + "-" + "<color=yellow>DEV</color>");
        }

        public string getBuildTypeDescription()
        {
            if (ConfigApp.ProductionBuild)
            {
                return "release";
            }
            if (ConfigApp.IsStableBuild())
            {
                return "stable";
            }
            return "dev";
        }

        public string getCommitId()
        {
            return (string) this.CloudBuildManifest["scmCommitId"];
        }

        public string getPrettyCommitId()
        {
            string str = this.getCommitId();
            return ("#" + str.Substring(0, Mathf.Min(str.Length, 8)));
        }

        public Dictionary<string, object> CloudBuildManifest
        {
            [CompilerGenerated]
            get
            {
                return this.<CloudBuildManifest>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CloudBuildManifest>k__BackingField = value;
            }
        }
    }
}

