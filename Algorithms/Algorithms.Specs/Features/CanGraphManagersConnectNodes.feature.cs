﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.17929
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Percolation.Specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Graph manager can connect nodes")]
    public partial class GraphManagerCanConnectNodesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CanGraphManagersConnectNodes.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Graph manager can connect nodes", "In order to establish a graph\r\nAs a user\r\nI want to be able to connect nodes", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Node connections are Reflexive")]
        [NUnit.Framework.TestCaseAttribute("QuickFindGraphManager", "10", null)]
        [NUnit.Framework.TestCaseAttribute("QuickWeigthedUnionGraphManager", "10", null)]
        public virtual void NodeConnectionsAreReflexive(string graphManager, string n, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Node connections are Reflexive", exampleTags);
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given(string.Format("I have a {0} with {1} nodes", graphManager, n), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
  testRunner.Then(string.Format("number of nodes equals {0}", n), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
   testRunner.And("each node should be connected to itself", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Node connections are Symmetric")]
        [NUnit.Framework.TestCaseAttribute("QuickFindGraphManager", "10", "5", "6", null)]
        [NUnit.Framework.TestCaseAttribute("QuickWeigthedUnionGraphManager", "10", "1", "9", null)]
        public virtual void NodeConnectionsAreSymmetric(string graphManager, string n, string a, string b, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Node connections are Symmetric", exampleTags);
#line 15
this.ScenarioSetup(scenarioInfo);
#line 16
 testRunner.Given(string.Format("I have a {0} with {1} nodes", graphManager, n), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
  testRunner.When(string.Format("I connect node {0} with node {1}", a, b), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
  testRunner.Then(string.Format("node {0} should be connected to node {1}", a, b), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
   testRunner.And(string.Format("node {0} should be connected to node {1}", b, a), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Node connections are Transitive")]
        [NUnit.Framework.TestCaseAttribute("QuickFindGraphManager", "10", "2", "5", "9", null)]
        [NUnit.Framework.TestCaseAttribute("QuickWeigthedUnionGraphManager", "10", "1", "2", "7", null)]
        public virtual void NodeConnectionsAreTransitive(string graphManager, string n, string a, string b, string c, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Node connections are Transitive", exampleTags);
#line 25
this.ScenarioSetup(scenarioInfo);
#line 26
 testRunner.Given(string.Format("I have a {0} with {1} nodes", graphManager, n), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
  testRunner.When(string.Format("I connect node {0} with node {1}", a, b), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
  testRunner.When(string.Format("I connect node {0} with node {1}", b, c), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
  testRunner.Then(string.Format("node {0} should be connected to node {1}", a, c), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
   testRunner.And(string.Format("node {0} should be connected to node {1}", c, a), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
