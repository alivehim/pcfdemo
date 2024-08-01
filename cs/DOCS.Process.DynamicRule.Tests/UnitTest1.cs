using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FakeXrmEasy.Plugins;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using System.Reflection;
using System.Threading.Tasks;
using DOCS.Process.DynamicRule.Plugin;
namespace DOCS.Process.DynamicRule.Tests
{
    [TestClass]
    public class UnitTest1: FakeXrmEasyTestsBase
    {
        [TestMethod]
        public void test_scenario_with_no_rules()
        {
            _context.EnableProxyTypes(Assembly.GetExecutingAssembly()); //Needed to be able to return early bound entities

            //Get a default plugin execution context that will invoke actions against the In-Memory database automatically
            var pluginContext = _context.GetDefaultPluginContext();

            //Set the necessary properties of the pluginExecution context that the plugin will need
            //var guid1 = Guid.NewGuid();
            //var target = new Entity("account") { Id = guid1 };

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add(Consts.Input_Rules, "{}");

            //ParameterCollection outputParameters = new ParameterCollection();
            //outputParameters.Add("id", guid1);

            pluginContext.InputParameters = inputParameters;
            //pluginContext.OutputParameters = outputParameters;

            //Execute the plugin with the fake PluginExecutionContext            
            _context.ExecutePluginWith<DynamicRulePlugin>(pluginContext);


            Assert.IsFalse(bool.Parse( pluginContext.OutputParameters[Consts.Output_Result].ToString()));
        }


        [TestMethod]
        public void test_scenario_with_rules()
        {
            _context.EnableProxyTypes(Assembly.GetExecutingAssembly()); //Needed to be able to return early bound entities

            //Get a default plugin execution context that will invoke actions against the In-Memory database automatically
            var pluginContext = _context.GetDefaultPluginContext();

            //Set the necessary properties of the pluginExecution context that the plugin will need
            //var guid1 = Guid.NewGuid();
            //var target = new Entity("account") { Id = guid1 };

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add(Consts.Input_Rules, @"[
    {
        ""ID"": ""d440b6d2-5fd4-4132-8b05-2873a28a807d"",
        ""ParentID"": """",
        ""Name"": ""root"",
        ""Operator"": 2,
        ""IsGroup"": true,
        ""Value"": 1,
        ""Type"": ""string"",
        ""TargetValue"": 1,
        ""LogicalOperator"": ""OR""
    },
    {
        ""ID"": ""e916f33a-ebfe-4181-b136-63471207ae2a"",
        ""ParentID"": ""d440b6d2-5fd4-4132-8b05-2873a28a807d"",
        ""Name"": ""Bryana"",
        ""Operator"": 1,
        ""IsGroup"": false,
        ""Value"": ""2"",
        ""Type"": ""string"",
        ""TargetValue"": ""2"",
        ""LogicalOperator"": ""AND""
    },
    {
        ""ID"": ""f8e3b15f-410d-4db9-8c4b-1515c8779179"",
        ""ParentID"": ""d440b6d2-5fd4-4132-8b05-2873a28a807d"",
        ""Name"": ""Hilary"",
        ""Operator"": 1,
        ""IsGroup"": false,
        ""Value"": ""3"",
        ""Type"": ""number"",
        ""TargetValue"": ""3"",
        ""LogicalOperator"": ""AND""
    }
]");

            //ParameterCollection outputParameters = new ParameterCollection();
            //outputParameters.Add("id", guid1);

            pluginContext.InputParameters = inputParameters;
            //pluginContext.OutputParameters = outputParameters;

            //Execute the plugin with the fake PluginExecutionContext            
            _context.ExecutePluginWith<DynamicRulePlugin>(pluginContext);


            Assert.IsTrue(bool.Parse(pluginContext.OutputParameters[Consts.Output_Result].ToString()));
        }
    }
}
