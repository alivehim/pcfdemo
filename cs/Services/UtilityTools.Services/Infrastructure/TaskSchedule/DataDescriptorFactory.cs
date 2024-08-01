using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DependencyInjection;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.TaskSchedule
{
    public class DataDescriptorFactory : IDataDescriptorFactory
    {
        //private Dictionary<MessageOwner, IDataDescriptor> dataDescriptors = new Dictionary<MessageOwner, IDataDescriptor> { };

        private readonly IEnumerable<ExtractDescriptionNode>  extractDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public DataDescriptorFactory(IContainerProvider containerProvider,IEnumerable<ExtractDescriptionNode>  extractDescriptionNodes)
        {
            this.extractDescriptionNodes = extractDescriptionNodes;
            this.containerProvider = containerProvider;
            //var types = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(p => p.BaseType != null
            //    && typeof(IDataDescriptor).IsAssignableFrom(p)
            //    && !p.IsAbstract
            //    && !p.Name.Contains("MultiPageDescriptor"));

            //foreach (var type in types)
            //{
            //    var descriptor = Activator.CreateInstance(type) as IDataDescriptor;

            //    var attribute = type.GetCustomAttribute<MessageOwnerAttribute>();

            //    dataDescriptors.Add(attribute.MessageOwner, descriptor);
            //}
        }

        //public IDataDescriptor GetPageDescriptor(string signal)
        //{
        //    return dataDescriptors.FirstOrDefault(p => p.Key.Contains(signal)).Value;
        //}

        public IDataDescriptor GetPageDescriptorByMessageOwner(MessageOwner messageOwner)
        {
            var endpoint = extractDescriptionNodes.Where(p => p.MessageOwner == messageOwner).FirstOrDefault();
            if (endpoint == null)
            {
                throw new Exception($"{messageOwner} handler not register");
            }

            return Resovle(endpoint);

        }

        private IDataDescriptor Resovle(ExtractDescriptionNode  extractDescriptionNode)
        {
            var handler = containerProvider.Resolve(extractDescriptionNode.Handler) as IDataDescriptor;
            return handler;
        }
    }
}
