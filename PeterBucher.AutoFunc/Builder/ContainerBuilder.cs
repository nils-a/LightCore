﻿using System;
using System.Collections.Generic;
using System.Linq;

using PeterBucher.AutoFunc.Exceptions;
using PeterBucher.AutoFunc.ExtensionMethods;
using PeterBucher.AutoFunc.Fluent;
using PeterBucher.AutoFunc.Properties;
using PeterBucher.AutoFunc.Reuse;

namespace PeterBucher.AutoFunc.Builder
{
    /// <summary>
    /// Represents a builder that is reponsible for accepting, validating registrations
    /// and builds the container with that registrations.
    /// </summary>
    public class ContainerBuilder : IContainerBuilder
    {
        /// <summary>
        /// Holds a list with registered registrations.
        /// </summary>
        private readonly IDictionary<RegistrationKey, Registration> _registrations;

        /// <summary>
        /// Holds a list with registering callbacks.
        /// </summary>
        private readonly IList<Action> _registrationCallbacks;

        /// <summary>
        /// Initializes a new instance of <see cref="ContainerBuilder" />.
        /// </summary>
        public ContainerBuilder()
        {
            this._registrations = new Dictionary<RegistrationKey, Registration>();
            this._registrationCallbacks = new List<Action>();
        }

        /// <summary>
        /// Registers a contract with its implementationtype.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation for the contract</typeparam>
        /// <returns>An instance of <see cref="IFluentRegistration"  /> that exposes methods for LifeTime altering.</returns>
        public IFluentRegistration Register<TContract, TImplementation>()
        {
            return this.Register(typeof(TContract), typeof(TImplementation));
        }

        /// <summary>
        /// Registers a contract with its implementationtype.
        /// </summary>
        /// <param name="typeOfContract">The type of the contract.</param>
        /// <param name="typeOfImplementation">The type of the implementation for the contract</param>
        /// <returns>An instance of <see cref="IFluentRegistration"  /> that exposes methods for LifeTime altering.</returns>
        public IFluentRegistration Register(Type typeOfContract, Type typeOfImplementation)
        {
            var key = new RegistrationKey(typeOfContract, typeOfImplementation, null);

            // Register the type with default lifetime.
            var registration = new Registration(typeOfContract, typeOfImplementation, key);
            
            // Set the transient reuse strategy as default.
            if(registration.ReuseStrategy == null)
            {
                registration.ReuseStrategy = new TransientReuseStrategy();
            }

            // Add a register callback for lazy assertion after manipulating in fluent registration api.
            this._registrationCallbacks.Add(() =>
                                                {
                                                    this.AssertRegistrationExists(registration.Key);
                                                    this._registrations.Add(registration.Key, registration);
                                                });

            // Return a new instance of <see cref="IFluentRegistration" /> for supporting a fluent interface for registration configuration.
            return registration.FluentRegistration;
        }

        /// <summary>
        /// Builds the container.
        /// </summary>
        /// <returns>The builded container.</returns>
        public IContainer Build()
        {
            // Invoke the callbacks, they assert if the registration already exists, if not, register the registration.
            this._registrationCallbacks.ForEach(registerCallback => registerCallback());

            return new Container(this._registrations);
        }

        /// <summary>
        /// Registers a module with registrations.
        /// </summary>
        /// <param name="module">The module.</param>
        public void RegisterModule(RegistrationModule module)
        {
            module.Register(this);
        }

        /// <summary>
        /// Asserts whether the registration already exists.
        /// </summary>
        /// <param name="registrationKey">The registration key to check for.</param>
        private void AssertRegistrationExists(RegistrationKey registrationKey)
        {
            var selectors = new List<Func<RegistrationKey, bool>>
                                {
                                    r => r.ContractType == registrationKey.ContractType,
                                    r => r.ImplementationType == registrationKey.ImplementationType,
                                    r => r.Name == registrationKey.Name
                                };

            Func<RegistrationKey, bool> registrationEqualsSelector = selectors.Aggregate((current, next) => r => current(r) && next(r));
            Func<RegistrationKey, bool> registrationNameEqualsSelector = r => r.Name != null && r.Name == registrationKey.Name;

            Func<RegistrationKey, bool> mainSelector = r => registrationEqualsSelector(r) || registrationNameEqualsSelector(r);

            // Check if the registration key already exists.
            if (this._registrations.Any(r => mainSelector(r.Key)))
            {
                throw new RegistrationAlreadyExistsException(
                    Resources.RegistrationForContractAndNameAlreadyExistsFormat.FormatWith(
                        registrationKey.ContractType,
                        registrationKey.Name));
            }
        }
    }
}