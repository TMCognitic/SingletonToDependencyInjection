using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Patterns.IOC
{
    public interface ISimpleIOC
    {
        void Register<TResource>();
        void Register<TResource>(Func<TResource> builder);
        void Register<TResource, TConcrete>()
            where TConcrete : TResource;
        void Register<TResource, TConcrete>(Func<TConcrete> builder)
            where TConcrete : TResource;

        TResource GetResource<TResource>();
        object GetResource(Type resourceType);
    }
}
