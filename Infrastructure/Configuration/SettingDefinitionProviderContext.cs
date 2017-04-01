namespace Infrastructure.Configuration
{
    /// <summary>
    /// The context that is used in setting providers.
    /// </summary>
    public class SettingDefinitionProviderContext
    {
        public ISettingDefinitionManager Manager { get; set; }

        internal SettingDefinitionProviderContext(ISettingDefinitionManager manager)
        {
            Manager = manager;
        }
    }
}
