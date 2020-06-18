using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ToolBox.Patterns.IOC;

namespace ToolBox.Patterns.Locator
{
    public abstract class LocatorBase
    {
        protected ISimpleIOC Container
        {
            get;
            private set;
        }        

        protected LocatorBase()
            : this(new SimpleIOC())
        {

        }

        protected LocatorBase(ISimpleIOC container)
        {
            Container = container;
            _resources = new DynamicResources(Container);
        }

        private DynamicResources _resources;

        public dynamic Resources
        {
            get
            {
                return _resources;
            }
        }

        private class DynamicResources : DynamicObject
        {
            private ISimpleIOC Container { get; }
            public DynamicResources(ISimpleIOC container)
            {
                Container = container;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {                
                try
                {
                    result = Container.GetResource((Type)indexes[0]);
                    return true;
                }
                catch (Exception ex)
                {
                    result = null;
                    return false;
                }
            }

            public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
            {                
                return false;
            }
        }
    }
}
