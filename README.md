# ExprShell

Expression shell (Will be documented soon)

**Preview**

```csharp
ExpressionDefinition definition = new ExpressionDefinition
{
    ExpressionGroup = new List<ExpressionGroup> {
        new ExpressionGroup {
            ExpressionList = new List < BasicExpression > {
                new MacroExpression {
                    Key = "Basket.Items",
                    Value = 10,
                    Operator = ExpressionOp.Gte,
                    MacroOp = new FilterMacroOpDefinition {
                        PropertyToVisit = "Product.Price",
                        Take = 2,
                        Op = MacroOp.SumOfMinXItem
                    }

                }
            },

            Op = ExpressionGroupOp.And
        }
    },

    Op = ExpressionGroupOp.And,
    ReturnType = ReturnTypes.Boolean
};
```

**GENERATED JS EXPRESSION**

```js
function ExpFunc(jsonParam) {
	var p = JSON.parse(jsonParam);
	return ((_.chain(p.Basket.Items)
		.pluck('Product.Price')
		.sortByOrder(false)
		.take(2)
		.sum()
		.value()) >= 10);
}

```

**EVALUATE EXPRESSIN WITH PARAMETERS**

```csharp
Basket basket = FixtureRepository.Create<Basket>();

var parameters = new Dictionary<string, object>
            {
                {"Basket", basket}
            };

ExpressionEvalResult result =
                  _context
                  .Evaluate(expressionDefinition, parameters);

result.Should().NotBeNull();
bool.Parse(result.Value.ToString()).Should().BeTrue();
```
