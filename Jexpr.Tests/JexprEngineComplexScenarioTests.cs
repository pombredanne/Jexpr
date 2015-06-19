using System.Collections.Generic;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
using Jexpr.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class JexprEngineComplexScenarioTests : TestBase
    {
        private IJexprEngine _engine;

        protected override void FinalizeSetUp()
        {
            _engine = new JexprEngine();
        }

        [Description(@"FOR basket, profile, payment inforrmation =>
                             BoutiqueId should be one of [12, 14]  
                       AND   Brand should be one of [Adidas]
                       AND   BankBin should be one of [Garanti, Teb, Finans] 
                       AND   Age should greater than equal 20
                       RET   ( APPLY 20% to Total Basket Price)")]
        [Test]
        public void ComplexScenario1_Test()
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
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            string json2 = JsonConvert.SerializeObject(metadata, settings);

            JexprResult<TestApplyResult> result = _engine.Evaluate<TestApplyResult>(metadata, parameters);
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Discount.Should().BeGreaterThan(0m);
        }

        [Description(@"FOR basket =>
                             Brand should be one of [Adidas]
                       RET   ( APPLY 20% to Total Basket Price)")]
        [Test]
        public void ComplexScenario2_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            basket.Products[0].Parameters.Add("BoutiqueId", 12);
            basket.Products[1].Parameters.Add("BoutiqueId", 12);
            basket.Products[2].Parameters.Add("BoutiqueId", 18);

            basket.Products[0].Parameters.Add("Brand", "Adidas");
            basket.Products[1].Parameters.Add("Brand", "Nike");
            basket.Products[2].Parameters.Add("Brand", "Nike");

            var metadata = TestDataBuilder.GetMacroExprMetadata4ComplexScenarion2();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket},
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            JexprResult<TestApplyResult> result = _engine.Evaluate<TestApplyResult>(metadata, parameters);
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Discount.Should().BeGreaterThan(0m);
        }

        [Test]
        public void ComplexScenario_With_MultiplePromotions_Test()
        {
            List<ExpressionMetadata> metadataList = new List<ExpressionMetadata>
            {
                TestDataBuilder.GetMacroExprMetadata4ComplexScenarion1(),
                TestDataBuilder.GetMacroExprMetadata4ComplexScenarion2()
            };

            Basket basket = FixtureRepository.Create<Basket>();

            basket.Products[0].Parameters.Add("BoutiqueId", 12);
            basket.Products[1].Parameters.Add("BoutiqueId", 12);
            basket.Products[2].Parameters.Add("BoutiqueId", 18);        
            
            basket.Products[0].Parameters.Add("Id", 1);
            basket.Products[1].Parameters.Add("Id", 2);
            basket.Products[2].Parameters.Add("Id", 3);

            basket.Products[0].Parameters.Add("Brand", "Nike");
            basket.Products[1].Parameters.Add("Brand", "Adidas");
            basket.Products[2].Parameters.Add("Brand", "Adidas");


            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket},
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            List<JexprResult<TestApplyResult>> results = new List<JexprResult<TestApplyResult>>();

            foreach (ExpressionMetadata metadata in metadataList)
            {
                JexprResult<TestApplyResult> result = _engine.Evaluate<TestApplyResult>(metadata, parameters);
                result.Should().NotBeNull();
                result.Value.Should().NotBeNull();
                result.Value.Discount.Should().BeGreaterThan(0m);

                results.Add(result);
            }

            results.Count.Should().Be(metadataList.Count);
        }
    }
}