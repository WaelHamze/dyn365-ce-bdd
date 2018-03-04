using Common;
using Microsoft.Dynamics365.UIAutomation.Api;
using Microsoft.Dynamics365.UIAutomation.Api.Pages;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class Steps
    {
        private readonly BrowserOptions options = new BrowserOptions
        {
            BrowserType = BrowserType.Chrome,
            PrivateMode = true,
            FireEvents = true
        };

        private XrmBrowser xrmBrowser;
        private RandomGenerator random = new RandomGenerator();

        [Given(@"I login to CRM")]
        public void GivenILoginToCRM()
        {
            string url = Environment.GetEnvironmentVariable("CrmUrl", EnvironmentVariableTarget.User);
            string user = Environment.GetEnvironmentVariable("CrmUsername", EnvironmentVariableTarget.User);
            string pass = Environment.GetEnvironmentVariable("CrmPassword", EnvironmentVariableTarget.User);

            xrmBrowser = new XrmBrowser(options);
            xrmBrowser.LoginPage.Login(new Uri(url), user.ToSecureString(), pass.ToSecureString());
            xrmBrowser.GuidedHelp.CloseGuidedHelp();

            xrmBrowser.ThinkTime(500);
        }

        [AfterScenario()]
        public void CleanUp()
        {
            if (xrmBrowser != null)
            {
                xrmBrowser.Dispose();
            }
        }
        
        [When(@"I navigate to (.*) and select (.*)")]
        public void WhenINavigateTo(string area, string subArea)
        {
            xrmBrowser.Navigation.OpenSubArea(area, subArea);
            xrmBrowser.ThinkTime(500);
        }
        
        [Then(@"I click on (.*) command")]
        public void WhenIClickCommand(string command)
        {
            xrmBrowser.CommandBar.ClickCommand(command);
            xrmBrowser.ThinkTime(1000);
        }

        [Then(@"I set value of (.*) to RandomString of (.*)")]
        public void ThenISetValueToRandomString(string field, int length)
        {
            xrmBrowser.Entity.SetValue(field, random.GetString(length));
            xrmBrowser.ThinkTime(500);
        }

        [Then(@"I set value of (.*) to '(.*)'")]
        public void ThenISetValue(string field, string value)
        {
            xrmBrowser.Entity.SetValue(field, value);
            xrmBrowser.ThinkTime(500);
        }

        [When(@"I go to social pane and select (.*)")]
        public void WhenIGoToSocialPaneAndSelect(string tab)
        {
            xrmBrowser.ActivityFeed.SelectTab(XrmActivityFeedPage.Tab.Activities);
            xrmBrowser.ThinkTime(500);
        }

        [Then(@"Activity timeline count should be equal to (.*)")]
        public void ThenGridRecordCountShouldBeEqualTo(int count)
        {
            List<XrmGridItem> items = xrmBrowser.ActivityFeed.GetGridItems().Value;
            Assert.AreEqual(items.Count, count);
        }
    }
}
