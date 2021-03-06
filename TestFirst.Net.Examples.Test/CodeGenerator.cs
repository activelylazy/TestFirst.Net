﻿using System;
using NUnit.Framework;
using TestFirst.Net.Examples.Api;

namespace TestFirst.Net.Examples
{
    [TestFixture]
    public class MatcherCodeGenerator
    {
        public static void Main(string[] args)
        {
            new MatcherCodeGenerator().GenerateCode();
        }
        
        [Test]
        public void Generate()
        {
            GenerateCode();
        }

        private void GenerateCode()
        {
            var template = new Template.MatchersTemplate();

            template.Defaults().WithNamespace("TestFirst.Net.Examples");
            
            //// template.ForPropertyType<Classifiers>()
            ////    .AddMatchMethodTaking<string>("$propertyName(AClassifier.EqualTo($argName));")
            ////    .AddMatchMethodTaking<Classifiers>("$propertyName(AClassifier.EqualTo($argName));");

            //// template.ForPropertyType<NRequire.Model.Version>()
            ////    .AddMatchMethodTaking<string>("$propertyName(NRequire.Model.Version.Parse($argName));")
            ////    .AddMatchMethodTaking<NRequire.Model.Version>("$propertyName(AnInstance.EqualTo($argName));");

            //// template.ForPropertyType<VersionMatcher>()
            ////    .AddMatchMethodTaking<string>("$propertyName(Matchers.Function<VersionMatcher>(actual => actual.ToString().Equals($argName), () => $argName));")
            ////    .AddMatchMethodTaking<NRequire.Model.Version>("$propertyName($argName.ToString());");

            Console.WriteLine("Starting generation");
            
            template.GenerateForAssembly(typeof(Notification).Assembly, "TestFirst.Net.Examples.Api.*");

            template.RenderToFile("Matchers.generated.cs");
        }
    }
}
