using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins
{
    public class AccountPostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference.
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                var target = context.InputParameters["Target"] as Entity;

                Entity phonecall = new Entity("phonecall");
                phonecall["regardingobjectid"] = target.ToEntityReference();
                phonecall["phonenumber"] = target["telephone1"];
                phonecall["scheduledend"] = DateTime.Now.AddDays(5);
                phonecall["subject"] = "Introductory phonecall";

                service.Create(phonecall);
            }
        }
    }
}
