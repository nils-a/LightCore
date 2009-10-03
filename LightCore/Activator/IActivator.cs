﻿using System.Collections.Generic;

namespace LightCore.Activator
{
    /// <summary>
    /// Represents an instance activator.
    /// </summary>
    public interface IActivator
    {
        /// <summary>
        /// Activates an instance with given arguments.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The activated instance.</returns>
        object ActivateInstance(IContainer container, IEnumerable<object> arguments);
    }
}