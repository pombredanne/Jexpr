using System.Collections.Generic;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Templates;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class TemplateEngineTests : TestBase
    {
        [Test]
        public void Transform_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            basket.Products[0].Parameters.Add("BoutiqueId", 12);
            basket.Products[1].Parameters.Add("BoutiqueId", 12);
            basket.Products[2].Parameters.Add("BoutiqueId", 18);

            basket.Products[0].Parameters.Add("Brand", "Adidas");
            basket.Products[1].Parameters.Add("Brand", "Nike");
            basket.Products[2].Parameters.Add("Brand", "Nike");



            var metadata = TestDataBuilder.GetMacroExprMetadata4ComplexScenarion1();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket},
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
