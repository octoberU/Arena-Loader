using MelonLoader;
using System.Reflection;

namespace ArenaLoader
{
    public static class Config
    {
        public const string Category = "ArenaLoader";

        public static float bloomAmount;
        public static string lastArena;

        public static void RegisterConfig()
        {
            MelonPrefs.RegisterFloat(Category, nameof(bloomAmount), 0.5f, "Controls the amount of glow/bloom [0,3,0.1,0.5]");
            MelonPrefs.RegisterString(Category, nameof(lastArena), "environment1", "Last loaded arena");

            OnModSettingsApplied();
        }

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(Config).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.Name == "Category") continue;

                if (fieldInfo.FieldType == typeof(float))
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat(Category, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(string))
                    fieldInfo.SetValue(null, MelonPrefs.GetString(Category, fieldInfo.Name));
            }
        }
    }
}
