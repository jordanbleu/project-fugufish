namespace Assets.Source.Configuration.Base
{
    /// <summary>
    /// Base class for all Configuration Classes that can be saved / loaded via ConfigurationFactory.
    /// <para>
    /// Config classes should have all their members initialized to their default values.  This is because 
    /// if a configuration file does not exist, it will be initialized with these defaults
    /// </para>
    /// </summary>
    public abstract class ConfigurationBase
    {
        // No abstract members needed for now
    }
}
