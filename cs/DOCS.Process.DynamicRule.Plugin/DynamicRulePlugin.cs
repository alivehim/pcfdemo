using DOCS.Process.DynamicRule.Plugin.Core;
using DOCS.Process.DynamicRule.Plugin.Extensions;
using DOCS.Process.DynamicRule.Plugin.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin
{
    public class DynamicRulePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            // Obtain the target entity from the input parameters.  
            //Entity request = (Entity)context.InputParameters[Consts.Target];



            // Obtain the organization service reference which you will need for web service calls.  
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            string rules = (string)context.InputParameters[Consts.Input_Rules];
            try
            {

                var ruleList = rules.DeserializeObject<IList<DocsRule>>();

                if (ruleList == null || ruleList.Count == 0)
                {
                    context.OutputParameters[Consts.Output_Result] = false;
                    return;
                }

                var ruleGroup = ruleList.Convert2RuleGroup();

                var expressionParser = new ExpressionParser();
                var predicate = expressionParser
                    .ParsePredicateOf(ruleGroup);

                var responseResult = new ResponseResult()
                {
                    code = 1,
                    result = predicate()
                };
                context.OutputParameters[Consts.Output_Result] = responseResult.SerializeObject();


            }
            //catch (FaultException<OrganizationServiceFault> ex)
            //{
            //    //NotificationHelper.Me.LogError(service, ex, entity.Id);
            //    throw new InvalidPluginExecutionException("An error occurred in DynamicRulePlugin. " + ex.ToString(), ex);
            //}
            catch (Exception ex)
            {
                //NotificationHelper.Me.LogError(service, ex, entity.Id);
                tracingService.Trace("DynamicRulePlugin: {0}", ex.ToString());

                var responseResult = new ErrorResponse()
                {
                    code = 0,
                    message = ex.ToString()
                };
                context.OutputParameters[Consts.Output_Result] = responseResult.SerializeObject();

            }
        }
    }
}
