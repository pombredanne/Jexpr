using System.Collections.Generic;
using System.Linq;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class ExpressionEvalContextTests : TestBase
    {
        private IExpressionEvalContext _context;
        private const int TEST_INT_PARAM = 12;

        protected override void FinalizeSetUp()
        {
            _context = new ExpressionEvalContext();
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

            ExpressionEvalResult result = _context.Evaluate(expressionDefinition, parameters);

            bool.Parse(result.Value.ToString()).Should().BeFalse();
        }

        [Test]
        public void Macro_Expression_Eval_Test()
        {
            var expressionDefinition = TestDataBuilder.GetExprDefinition4Basket();

            Basket basket = FixtureRepository.Create<Basket>();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            ExpressionEvalResult result = _context.Evaluate(expressionDefinition, parameters);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Expression_Group_Eval_Test()
        {
            var expression = TestDataBuilder.GetExprDefinition();
            expression.Groups.AddRange(TestDataBuilder.GetExprDefinition4Basket().Groups);

            var parameters = new Dictionary<string, object>
            {
                {"BoutiqueId", 258},
                {"BasketTotal", 62},
                {"CodeCampaign", 56},
                {"BankBin", "Garanti"},
                {"SavedCreditCard", 1},
                {"Basket", FixtureRepository.Create<Basket>()}
            };

            ExpressionEvalResult result = _context.Evaluate(expression, parameters);

            bool.Parse(result.Value.ToString()).Should().BeFalse();
        }

        [Test]
        public void Advanced_Expression_Group_Eval_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int max = (int)basket.Items.Select(item => item.Product.Price).Max(arg => arg);

            var definition = TestDataBuilder.GetExprDefinition();
            definition.Groups.AddRange(TestDataBuilder.GetExprDefinition4Basket().Groups);
            definition.ResultExpression = TestDataBuilder.GetResultExpression(max);
            definition.ReturnType = ReturnTypes.JsonString;

            var parameters = new Dictionary<string, object>
            {
                {"BoutiqueId", 258},
                {"BasketTotal", 62},
                {"CodeCampaign", 56},
                {"BankBin", "Garanti"},
                {"SavedCreditCard", 1},
                {"Basket", FixtureRepository.Create<Basket>()}
            };

            ExpressionEvalResult result = _context.Evaluate(definition, parameters);

            ((JsFuncResult)result.Value).Success.Should().BeFalse();
        }

        [Test]
        public void Eq_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Eq, TEST_INT_PARAM);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Neq_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Neq, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Gt_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Gt, TEST_INT_PARAM + 1);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Lt_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Lt, TEST_INT_PARAM - 1);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Gte_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Gte, TEST_INT_PARAM);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Lte_Expression_Test()
        {
            ExpressionEvalResult result = GetExprEvalResult(ExpressionOp.Lte, TEST_INT_PARAM);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void In_Expression_Test()
        {
            var expression = TestDataBuilder.GetBasicExprDef(TEST_INT_PARAM, ExpressionOp.In, ReturnTypes.Boolean, "BoutiqueId");

            var parameters = new Dictionary<string, object> { { "BoutiqueId", new List<int> { TEST_INT_PARAM, 58, 69 } } };

            ExpressionEvalResult result = _context.Evaluate(expression, parameters);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Min_Expression_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int min = (int)basket.Items.Select(item => item.Product.Price).Min(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(min, ExpressionOp.Eq, ReturnTypes.Boolean, "Basket.Items", MacroOp.Min);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            ExpressionEvalResult result = _context.Evaluate(expression, parameters);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        [Test]
        public void Max_Expression_Test()
        {
            Basket basket = FixtureRepository.Create<Basket>();

            int max = (int)basket.Items.Select(item => item.Product.Price).Max(arg => arg);

            var expression = TestDataBuilder.GetMacroExprDef4MinMax(max, ExpressionOp.Eq, ReturnTypes.Boolean, "Basket.Items", MacroOp.Max);

            var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

            ExpressionEvalResult result = _context.Evaluate(expression, parameters);

            result.Should().NotBeNull();
            bool.Parse(result.Value.ToString()).Should().BeTrue();
        }

        private ExpressionEvalResult GetExprEvalResult(ExpressionOp op, object paramValue)
        {
            var expression = TestDataBuilder.GetBasicExprDef(TEST_INT_PARAM, op, ReturnTypes.Boolean, "BoutiqueId");

            var parameters = new Dictionary<string, object> { { "BoutiqueId", paramValue } };

            ExpressionEvalResult result = _context.Evaluate(expression, parameters);

            return result;
        }
    }
}