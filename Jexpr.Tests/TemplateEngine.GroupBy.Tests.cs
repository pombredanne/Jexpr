using System.Collections.Generic;
using System.Linq;
using Common.Testing.NUnit;
using FluentAssertions;
using Jexpr.Core;
using Jexpr.Core.Impl;
using Jexpr.Models;
using Jexpr.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Jexpr.Tests
{
    public class TemplateEngineGroupByTests : TestBase
    {
        private IJexprEngine _engine;
        protected override void FinalizeSetUp()
        {
            _engine = new JexprEngine();
        }

        [Test]
        public void GroupBy_Expression_Test()
        {
            //http://stackoverflow.com/questions/10022156/underscore-js-groupby-multiple-values

            TestBasket testBasket = FixtureRepository.Create<TestBasket>();

            testBasket.Products[0].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[1].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[2].Parameters.Add("BoutiqueId", 13);

            var groups = testBasket.Products.GroupBy(item => int.Parse(item.Parameters["BoutiqueId"].ToString())).Select(items => new { BoutiqueId = items.Key, TotalPrice = items.Sum(item => item.UnitPrice * item.Quantity) });

            var expression = TestDataBuilder.GetMacroExprDef4GroupBy();

            var parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket}
            };

            string json = JsonConvert.SerializeObject(parameters);

            JexprResult<dynamic> result = _engine.Evaluate<dynamic>(expression, parameters);
            JToken token = JToken.Parse(result.Value.ToString());
            JArray jArray = (JArray)token["value"];

            result.Should().NotBeNull();
            jArray.Should().NotBeNull();

            Assert.AreEqual(groups.Count(), jArray.Count);
        }
    }
}
