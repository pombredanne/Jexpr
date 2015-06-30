using System.Collections.Generic;
using System.Linq;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
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
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.Equal, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void NotEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.NotEqual, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void GreaterThan_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.GreaterThan, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void LowerThan_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.LowerThan, TEST_INT_PARAM - 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void GreaterThanEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.GreaterThanOrEqual, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void LowerThanEqual_Expression_Test()
        {
            JexprResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.LowerThanEqual, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Exists_Expression_Test()
        {
            var expression = TestDataBuilder.GetInExprDef(TEST_INT_PARAM, ReturnTypes.Boolean, "BoutiqueId");

            Basket basket = FixtureRepository.Build<Basket>().With(b => b.Products, FixtureRepository.CreateMany<Product>(1).ToList()).Create();

            basket.Products[0].Parameters.Add("BoutiqueId", TEST_INT_PARAM);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket},
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
            Basket basket = FixtureRepository.Create<Basket>();

            int min = (int)basket.Products.Select(item => item.UnitPrice).Min(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(min, ExpressionOp.Equal, ReturnTypes.Number, "Basket.Products", FilterOperator.Min);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, min);
        }

        [Test]
        public void Max_Expression_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int max = (int)basket.Products.Select(item => item.UnitPrice).Max(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(max, ExpressionOp.Equal, ReturnTypes.Number, "Basket.Products", FilterOperator.Max);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, max);
        }

        [Test]
        public void Engine_Eval_Js_Expression_Test()
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

            JexprJsGeneratorTemplate template = new JexprJsGeneratorTemplate(metadata);
            string js = template.TransformText();

            js.Should().NotBeNullOrEmpty();
            js.Should().NotBeNullOrWhiteSpace();

            JexprResult<TestApplyResult> result = _engine.Evaluate<TestApplyResult>(js, parameters);
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Discount.Should().BeGreaterThan(0m);
        }

        private JexprResult<T> GetExprEvalResult<T>(ExpressionOp op, object paramValue)
        {
            var expression = TestDataBuilder.GetBasicExprDef(TEST_INT_PARAM, op, ReturnTypes.Boolean, "BoutiqueId");

            var parameters = new Dictionary<string, object> { { "BoutiqueId", paramValue } };

            JexprResult<T> result = _engine.Evaluate<T>(expression, parameters);

            return result;
        }
    }
}