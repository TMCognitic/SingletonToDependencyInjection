using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToolBox.Patterns.IOC
{
    internal class SimpleIOC : ISimpleIOC
    {
        private IDictionary<Type, object> _instances;
        private IDictionary<Type, Type> _binders;
        private IDictionary<Type, Func<object>> _builders;

        public SimpleIOC()
        {
            _instances = new Dictionary<Type, object>();
            _binders = new Dictionary<Type, Type>();
            _builders = new Dictionary<Type, Func<object>>();
        }

        public TResource GetResource<TResource>()
        {
            return (TResource)GetResource(typeof(TResource));
        }

        public object GetResource(Type resourceType)
        {
            if (!_instances.ContainsKey(resourceType))
                throw new InvalidOperationException("Please register your resource before use it...");

            if (_instances[resourceType] is null)
            {
                if (_builders.TryGetValue(resourceType, out Func<object> builder))
                {
                    _instances[resourceType] = builder();
                }
                else
                {
                    _instances[resourceType] = Resolve(resourceType);
                }
            }

            return _instances[resourceType];
        }

        private object Resolve(Type resourceType)
        {
            if (!(_instances[resourceType] is null))
            {
                return _instances[resourceType];
            }
            else
            {
                if (_builders.TryGetValue(resourceType, out Func<object> builder))
                {
                    return builder();
                }

                Type concreteType = resourceType;
                if (_binders.ContainsKey(concreteType))
                    concreteType = _binders[concreteType];

                ConstructorInfo constructorInfo = concreteType.GetConstructors().SingleOrDefault();

                if (!(constructorInfo is null))
                {
                    object[] parameters = constructorInfo.GetParameters()
                                                         .Select(p => Resolve(p.ParameterType))
                                                         .ToArray();

                    return (_instances[resourceType] = constructorInfo.Invoke(parameters));
                }

                PropertyInfo propertyInfo = concreteType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);

                if (propertyInfo != null)
                    return (_instances[resourceType] = propertyInfo.GetMethod.Invoke(null, null));

                FieldInfo fieldInfo = concreteType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);

                if (fieldInfo != null)
                    return (_instances[resourceType] = fieldInfo.GetValue(null));

                throw new InvalidOperationException($"Can't initialize the type {resourceType.Name}");
            }
        }

        public void Register<TResource>()
        {
            _instances.Add(typeof(TResource), null);
        }

        public void Register<TResource>(Func<TResource> builder)
        {
            Type resourceType = typeof(TResource);
            _instances.Add(resourceType, null);
            _builders.Add(resourceType, () => builder());
        }

        public void Register<TResource, TConcrete>()
            where TConcrete : TResource
        {
            Type resourceType = typeof(TResource);
            _instances.Add(resourceType, null);
            _binders.Add(resourceType, typeof(TConcrete));
        }

        public void Register<TResource, TConcrete>(Func<TConcrete> builder)
            where TConcrete : TResource
        {
            Type resourceType = typeof(TResource);
            _instances.Add(resourceType, null);
            _builders.Add(resourceType, () => builder());
        }
    }
}
