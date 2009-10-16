﻿using System.Collections.Generic;
using System.Configuration;

namespace LightCore.Configuration
{
    /// <summary>
    /// Represents the current configuration.
    /// </summary>
    public class LightCoreConfiguration
    {
        ///<summary>
        /// Gets or sets the default lifecycle.
        ///</summary>
        public string DefaultLifecycle
        {
            get;
            set;
        }

        ///<summary>
        /// Gets or sets the type aliases.
        ///</summary>
        public List<TypeAlias> TypeAliases
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the registrations.
        /// </summary>
        public List<Registration> Registrations
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCoreConfiguration" /> type.
        /// </summary>
        public LightCoreConfiguration()
        {
            this.TypeAliases = new List<TypeAlias>();
            this.Registrations = new List<Registration>();
        }

        /// <summary>
        /// Gets the configuration instance.
        /// </summary>
        /// <value>The configuration instance.</value>
        public static LightCoreConfiguration Instance
        {
            get
            {
                var configSectionHandler =
                    (XamlConfigSectionHandler)ConfigurationManager.GetSection("LightCoreConfiguration");

                return configSectionHandler.GetInstance<LightCoreConfiguration>();
            }
        }
    }
}