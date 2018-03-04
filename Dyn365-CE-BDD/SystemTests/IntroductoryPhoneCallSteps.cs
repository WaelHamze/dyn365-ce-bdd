using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using TechTalk.SpecFlow;

namespace SystemTests
{
    [Binding]
    public class IntroductoryPhoneCallSteps
    {
        private CrmServiceClient service;
        private Entity account;
        private Entity phonecall;

        [Given(@"I login to CRM")]
        public void GivenILoginToCRM()
        {
            string url = Environment.GetEnvironmentVariable("CrmUrl", EnvironmentVariableTarget.User);
            string user = Environment.GetEnvironmentVariable("CrmUsername", EnvironmentVariableTarget.User);
            string pass = Environment.GetEnvironmentVariable("CrmPassword", EnvironmentVariableTarget.User);

            string con = $"AuthType=Office365;Username={user};Password={pass};Url={url}";

            service = new CrmServiceClient(con);
        }
        
        [When(@"I create a new account")]
        public void WhenICreateANewAccount()
        {
            account = new Entity("account");
            account.Attributes["name"] = new RandomGenerator().GetString(8);
            account["telephone1"] = "123456789";
            account.Id = service.Create(account);
        }
        
        [Then(@"a phone call record should be created")]
        public void ThenAPhoneCallRecordShouldBeCreated()
        {
            QueryByAttribute query = new QueryByAttribute("phonecall");
            query.Attributes.Add("regardingobjectid");
            query.Values.Add(account.Id);
            query.ColumnSet = new ColumnSet("scheduledend", "createdon", "activityid");
            EntityCollection results = service.RetrieveMultiple(query);
            Assert.AreEqual(results.TotalRecordCount, 1);
            phonecall = results[0];
        }
        
        [Then(@"due date should be in (.*) days")]
        public void ThenDueDateShouldBeInDays(int days)
        {
            DateTime dueDate = (DateTime)phonecall["scheduledend"];
            DateTime createdOn = (DateTime)phonecall["createdon"];
            Assert.AreEqual(dueDate.Date, createdOn.Date.AddDays(days));
        }
    }
}
