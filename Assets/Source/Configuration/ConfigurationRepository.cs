using Assets.Source.Configuration.Factory;

namespace Assets.Source.Configuration
{
    // todo:  I'm not sure if i like this.   I like the idea of having each scene be loaded completely independently, 
    // without persisting any data between the scenes.  This prevents issues with stale references and stuff.

    public static class ConfigurationRepository
    {
        public static SystemConfiguration SystemConfiguration { get; private set; }

        public static GamepadBindings GamepadBindings { get; private set; }
        public static KeyboardBindings KeyboardBindings { get; private set; }

        /// <summary>
        /// Reloads all configurations from disk into memory
        /// </summary>
        public static void RefreshAll()
        {
            ConfigurationFactory factory = new ConfigurationFactory();
            SystemConfiguration = factory.LoadOrDefault<SystemConfiguration>();
            GamepadBindings = factory.LoadOrDefault<GamepadBindings>();
            KeyboardBindings = factory.LoadOrDefault<KeyboardBindings>();

        }
    }
}
