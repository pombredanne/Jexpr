using System.Collections.Generic;
using System.Linq;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class ExpressionEvalContextTests : TestBase
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

            EvalResult<bool> result = _engine.Evaluate<bool>(expressionDefinition, parameters);

            Assert.IsFalse(result.Value);
        }

        [Test]
        public void Eq_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.Equal, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Neq_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.NotEqual, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Gt_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.GreaterThan, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Lt_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.LowerThan, TEST_INT_PARAM - 1);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Gte_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.GreaterThenOrEqual, TEST_INT_PARAM);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Lte_Expression_Test()
        {
            EvalResult<bool> result = GetExprEvalResult<bool>(ExpressionOp.LowerThanEqual, TEST_INT_PARAM);

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

            EvalResult<bool> result = _engine.Evaluate<bool>(expression, parameters);

            result.Should().NotBeNull();
            Assert.IsTrue(result.Value);
        }

        [Test]
        public void Min_Expression_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int min = (int)basket.Products.Select(item => item.UnitPrice).Min(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(min, ExpressionOp.Equal, ReturnTypes.Number, "Basket.Products", MacroOp.Min);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            EvalResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, min);
        }

        [Test]
        public void Max_Expression_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int max = (int)basket.Products.Select(item => item.UnitPrice).Max(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(max, ExpressionOp.Equal, ReturnTypes.Number, "Basket.Products", MacroOp.Max);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            EvalResult<double> result = _engine.Evaluate<double>(expression, parameters);

            result.Should().NotBeNull();
            Assert.AreEqual(result.Value, max);
        }

        [Test]
        public void GroupBy_Expression_Test()
        {
            //http://stackoverflow.com/questions/10022156/underscore-js-groupby-multiple-values

            Basket basket = FixtureRepository.Create<Basket>();

            basket.Products[0].Parameters.Add("BoutiqueId", 12);
            basket.Products[1].Parameters.Add("BoutiqueId", 12);
            basket.Products[2].Parameters.Add("BoutiqueId", 13);

            var groups = basket.Products.GroupBy(item => int.Parse(item.Parameters["BoutiqueId"].ToString())).Select(items => new { BoutiqueId = items.Key, TotalPrice = items.Sum(item => item.UnitPrice * item.Quantity) });

            var expression = TestDataBuilder.GetMacroExprDef4GroupBy();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            EvalResult<dynamic> result = _engine.Evaluate<dynamic>(expression, parameters);
            JToken token = JToken.Parse(result.Value.ToString());
            JArray jArray= (JArray)token["value"];

            result.Should().NotBeNull();
            jArray.Should().NotBeNull();

            Assert.AreEqual(groups.Count(), jArray.Count);
        }

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
                {
                    "Parameters",
                    new Dictionary<string, object>
                    {
                        {"BankBin", "Garanti"},
                        {"Age", 20}
                    }
                }
            };

            string json = JsonConvert.SerializeObject(parameters);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            string json2 = JsonConvert.SerializeObject(metadata, settings);

            

            EvalResult<PromotionApplyResult> result = _engine.Evaluate<PromotionApplyResult>(metadata, parameters);
            result.Should().NotBeNull();

        }

        private EvalResult<T> GetExprEvalResult<T>(ExpressionOp op, object paramValue)
        {
            var expression = TestDataBuilder.GetBasicExprDef(TEST_INT_PARAM, op, ReturnTypes.Boolean, "BoutiqueId");

            var parameters = new Dictionary<string, object> { { "BoutiqueId", paramValue } };

            EvalResult<T> result = _engine.Evaluate<T>(expression, parameters);

            return result;
        }
    }
}