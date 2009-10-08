﻿using LightCore.Activation;
using LightCore.Fluent;
using LightCore.Scope;

namespace LightCore
{
    /// <summary>
    /// Represents a registration.
    /// </summary>
    public class Registration
    {
        /// <summary>
        /// Gets the key for this registration.
        /// </summary>
        public RegistrationKey Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the activator.
        /// </summary>
        public IActivator Activator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scope that holds the reuse strategy.
        /// </summary>
        public IScope Scope
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the arguments for object creations.
        /// </summary>
        public object[] Arguments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current fluent registration interface instance.
        /// </summary>
        public IFluentRegistration FluentRegistration
        {
            get
            {
                return new FluentRegistration(this);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="Registration" />.
        /// </summary>
        public Registration()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="Registration" />.
        /// </summary>
        /// <param name="key">The registration key as <see cref="RegistrationKey" />.</see></param>
        public Registration(RegistrationKey key)
        {
            this.Key = key;
        }
    }
}