using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    /// <summary>
    /// Represents value of a setting.
    /// </summary>
    public interface ISettingValue
    {
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        string Value { get; }
    }
}
