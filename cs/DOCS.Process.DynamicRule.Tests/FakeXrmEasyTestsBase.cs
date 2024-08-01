using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Middleware;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.FakeMessageExecutors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DOCS.Process.DynamicRule.Tests
{
    [TestClass]
    public class FakeXrmEasyTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationService _service;

        public FakeXrmEasyTestsBase()
        {
            _context = MiddlewareBuilder
                        .New()

                        // Add* -> Middleware configuration
                        .AddCrud()
                        .AddFakeMessageExecutors(Assembly.GetAssembly(typeof(AddListMembersListRequestExecutor)))

                        // Use* -> Defines pipeline sequence

                        .UseCrud()
                        .UseMessages()

                        .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                        .Build();

            _service = _context.GetOrganizationService();
        }
    }
}
