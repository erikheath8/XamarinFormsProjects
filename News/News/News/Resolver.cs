using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Autofac;
using IContainer = Autofac.IContainer;

namespace News
{
    public static class Resolver
    {
        private static IContainer container;

        public static void Initialize(IContainer container)
        {
            Resolver.container = container;
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }


    }
}
