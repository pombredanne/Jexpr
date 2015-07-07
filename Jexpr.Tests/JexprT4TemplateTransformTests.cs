using System.Collections.Generic;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Templates;
using Jexpr.Tests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class JexprT4TemplateTransformTests : TestBase
    {
        [Test]
        public void Transform_Test()
        {
            TestBasket testBasket = FixtureRepository.Create<TestBasket>();

            testBasket.Products[0].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[1].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[2].Parameters.Add("BoutiqueId", 18);

            testBasket.Products[0].Parameters.Add("Brand", "Adidas");
            testBasket.Products[1].Parameters.Add("Brand", "Nike");
            testBasket.Products[2].Parameters.Add("Brand", "Nike");



            var metadata = TestDataBuilder.GetMacroExprMetadata4ComplexScenarion1();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket},
                {
                    "Parameters",
                    new Dictionary<string, object>
                    {
                        {"BankBin", "Garanti"},
                        {"Age", 20}
                    }
                }
            };

            var parametersObject = JsonConvert.SerializeObject(parameters);
            var serializeObject = JsonConvert.SerializeObject(metadata);
            JexprJsGeneratorTemplate template = new JexprJsGeneratorTemplate(metadata);
            string transformText = template.TransformText();

            transformText.Should().NotBeNullOrEmpty();
            transformText.Should().NotBeNullOrWhiteSpace();
        }
    }
}
