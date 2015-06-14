# Jexpr

[![Build status](https://ci.appveyor.com/api/projects/status/vrhmd31v1g86avo8/branch/master?svg=true)](https://ci.appveyor.com/project/ziyasal/jexpr/branch/master)

**Jexpr** is an experimental expression engine that creates javascript code from custom definitions to evaluate expressions. It uses [Jint](https://github.com/sebastienros/jint "Javascript Interpreter for .NET") and [Lodash](https://github.com/lodash/lodash "A JavaScript utility library delivering consistency, modularity, performance, & extras.") internally.

**Preview**

```csharp
 ExpressionDefinition expressionDefinition = new ExpressionDefinition {
 	Groups = new List < ExpressionGroup > {
 		new ExpressionGroup {
 			Items = new List <JexprExpression> {
 				new JexprMacroExpression {
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
var _engine = new JexprEngine ();

Basket basket = FixtureRepository.Create<Basket> ();
var parameters = new Dictionary <string,object> {
	{
		"Basket", basket
	}
};

EvalResult result = _engine.Evaluate(expression, parameters);

result.Should().NotBeNull();
bool.Parse(result.Value.ToString()).Should().BeTrue();
```
