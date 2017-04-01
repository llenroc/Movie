using Infrastructure.JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure.Modules
{
    /// <summary>
    /// Used to store all needed information for a module.
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// The assembly which contains the module definition.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Type of the module.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Instance of the module.
        /// </summary>
        public InfrastructureModule Instance { get; }

        /// <summary>
        /// All dependent modules of this module.
        /// </summary>
        public List<ModuleInfo> Dependencies { get; }

        /// <summary>
        /// Creates a new ModuleInfo object.
        /// </summary>
        public ModuleInfo([NotNull] Type type, [NotNull] InfrastructureModule instance)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            Type = type;
            Instance = instance;
            Assembly = Type.Assembly;

            Dependencies = new List<ModuleInfo>();
        }

        public override string ToString()
        {
            return Type.AssemblyQualifiedName ??Type.FullName;
        }
    }
}
