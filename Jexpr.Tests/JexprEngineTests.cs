using System.Collections.Generic;
using System.Linq;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
using Jexpr.Operators;
using Jexpr.Templates;
using Jexpr.Tests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class JexprEngineTests : TestBase
    {
        private IJexprEngine _engine;
        private const int TEST_INT_PARAM = 12;

        protected override void FinalizeSetUp()
        {
            _engine = new JexprEngine();
        }

        [Test]
        public void Basic_Expression_Build_Test()
        {
            var expressionDefinition = TestDataBuilder.GetExprDefinition();

            var parameters = new Dictionary<string, object>
            {
                {"BoutiqueId", 258},
                {"BasketTotal", 62},
                {"CodeCampaign", 56},
                {"BankBin", "Garanti"},
                {"SavedCreditCard", 1}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<bool> result = _engine.Evaluate<bool>(expressionDefinition, parameters);

            Assert.IsFalse(result.Value);
        }

        [Test]
        public void Equal_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.Equal, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void NotEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.NotEqual, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void GreaterThan_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.GreaterThan, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void LowerThan_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.LowerThan, TEST_INT_PARAM - 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void GreaterThanEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.GreaterThanOrEqual, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void LowerThanEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ConditionOperator.LowerThanEqual, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Exists_Expression_Test()
        {
            var expression = TestDataBuilder.GetInExprDef(TEST_INT_PARAM, "BoutiqueId");

            TestBasket testBasket = FixtureRepository.Build<TestBasket>().With(b => b.Products, FixtureRepository.CreateMany<TestProduct>(1).ToList()).Create();

            testBasket.Products[0].Parameters.Add("BoutiqueId", TEST_INT_PARAM);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket},
                {
                    "Parameters",
                    new Dictionary<string, object>
                    {
                        {"BoutiqueId", TEST_INT_PARAM},
                        {"Brand", "Adidas"},
                        {"BankBin", "Garanti"},
                        {"Age", 20}
                    }
                }
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<bool> result = _engine.Evaluate<bool>(expression, parameters);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Min_Expression_Test()
        {
            TestBasket testBasket = FixtureRepository.Create<TestBasket>();

            int min = (int)testBasket.Products.Select(item => item.UnitPrice).Min(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(min, ConditionOperator.Equal, "Basket.Products", max: false);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, min);
        }

        [Test]
        public void Max_Expression_Test()
        {
            TestBasket testBasket = FixtureRepository.Create<TestBasket>();

            int max = (int)testBasket.Products.Select(item => item.UnitPrice).Max(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(max, ConditionOperator.Equal, "Basket.Products", max: true);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, max);
        }

        [Test]
        public void Engine_Eval_Js_Expression_Test()
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
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            JexprJsGeneratorTemplate template = new JexprJsGeneratorTemplate(metadata);
            string js = template.TransformText();

            js.Should().NotBeNullOrEmpty();
            js.Should().NotBeNullOrWhiteSpace();

            JexprResult<TestApplyResult> result = _engine.Evaluate<TestApplyResult>(js, parameters);
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Discount.Should().BeGreaterThan(0m);
        }

        private JexprResult<T> GetExprEvalResult<T>(ConditionOperator op, object paramValue)
        {
            var expression = TestDataBuilder.GetBasicExprDef(TEST_INT_PARAM, op, "BoutiqueId");

            var parameters = new Dictionary<string, object> { { "BoutiqueId", paramValue } };

            JexprResult<T> result = _engine.Evaluate<T>(expression, parameters);

            return result;
        }
    }
}