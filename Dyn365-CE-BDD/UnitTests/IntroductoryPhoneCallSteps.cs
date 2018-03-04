using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Plugins;
using System;
using TechTalk.SpecFlow;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [Binding]
    public class IntroductoryPhoneCallSteps
    {
        private XrmFakedContext ctx;
        private Entity phonecall;

        [Given(@"I login to CRM")]
        public void GivenILoginToCRM()
        {
            ctx = new XrmFakedContext();
        }
        
        [When(@"I create a new account")]
        public void WhenICreateANewAccount()
        {
            Entity account = new Entity("account");
            account.Attributes["name"] = "Wael";
            account["telephone1"] = "123456789";

            ctx.ExecutePluginWithTarget<AccountPostCreate>(account);
        }
        
        [Then(@"a phone call record should be created")]
        public void ThenAPhoneCallRecordShouldBeCreated()
        {
            var phonecalls = ctx.CreateQuery("phonecall").ToList<Entity>();
            Assert.AreEqual(1, phonecalls.Count);
            phonecall = phonecalls[0];
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
