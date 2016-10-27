namespace Pathfinding.Serialization
{
    using System;

    public class SerializeSettings
    {
        public bool editorSettings;
        public bool nodes = true;
        public bool prettyPrint;

        public static SerializeSettings All
        {
            get
            {
                SerializeSettings settings = new SerializeSettings();
                settings.nodes = true;
                return settings;
            }
        }

        public static SerializeSettings Settings
        {
            get
            {
                SerializeSettings settings = new SerializeSettings();
                settings.nodes = false;
                return settings;
            }
        }
    }
}

