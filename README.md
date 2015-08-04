# Jexpr  
[![Build status](https://ci.appveyor.com/api/projects/status/vrhmd31v1g86avo8/branch/master?svg=true)](https://ci.appveyor.com/project/ziyasal/jexpr/branch/master) 
[![Coverage Status](https://coveralls.io/repos/ziyasal/Jexpr/badge.svg?branch=master&service=github)](https://coveralls.io/github/ziyasal/Jexpr?branch=master)

:warning::warning::warning: _It requires a lot of code refactoring to use in production_




**Jexpr** is an experimental expression engine that creates javascript code from custom definitions to evaluate expressions. It uses [Jint](https://github.com/sebastienros/jint "Javascript Interpreter for .NET") and [Lodash](https://github.com/lodash/lodash "A JavaScript utility library delivering consistency, modularity, performance, & extras.") internally.

**INTRODUCTION**
- [Pseudo Expression](#pseudo-expression)
- [Sample Parameters](#sample-parameters)
- [Csharp Code Sample](#csharp-code-sample)
- [Generated Js](#generated-js)

## Pseudo Expression
```sh
  WITH (`basket`, `profile`, `payment information`) DO
      BODY BEGIN
        FILTER `Basket.Products` DO
               `BoutiqueId` .should be one of.                [12, 14]  
          AND  `Brand`      .should be one of.                [Adidas]
        END
        IF WITH ( `Parameters`) DO
          AND  `BankBin`    .should be one of.                [Bank1, Bank2, Bank3] 
          AND  `Age`        .should be greater than or equal. 20
        END
      END
      RET BEGIN
        EXPR BEGIN
          APPLY `20%` TO `Total Basket Price`
          USING `UnitPrice` #It uses given field to calculate total
          SET   `Discount`
          SET   `Basket`    #It selects matched product group
        END
      END
  END
```

## Sample Parameters
```json
{
  "Basket": {
    "Products": [
      {
        "TotalPrice": 35208,
        "UnitPrice": 163,
        "Quantity": 216,
        "Parameters": {
          "BoutiqueId": 12,
          "Brand": "Adidas"
        }
      },
      {
        "TotalPrice": 19107,
        "UnitPrice": 99,
        "Quantity": 193,
        "Parameters": {
          "BoutiqueId": 12,
          "Brand": "Nike"
        }
      },
      {
        "TotalPrice": 23478,
        "UnitPrice": 129,
        "Quantity": 182,
        "Parameters": {
          "BoutiqueId": 18,
          "Brand": "Nike"
        }
      }
    ]
  },
  "Parameters": {
    "BankBin": "Garanti",
    "Age": 20
  }
}
```

##Csharp Code Sample
```csharp
ExpressionMetadata expressionMetadata = new ExpressionMetadata
{
    Items = new List<ExpressionGroup>
    {
        new ExpressionGroup
        {
            Criteria = new List<AbstractExpression>
            {
                new OperationExpression
                {
                    Key = "Basket.Products",
                    Filter = new SelectFilter
                    {
                            Conditions = new List<AbstractFilter>
                            {
                                new ConditionFilter("Parameters.BoutiqueId", 
                                ConditionOperator.Contains, new List<int> {12, 14}),
                                new ConditionFilter("Parameters.Brand", 
                                ConditionOperator.Contains, new List<string> {"Adidas"})
                            }
                        }
                },
                new OperationExpression
                {
                    Key = "Parameters.BankBin",
                    HasPriority = true,
                    Filter =new ConditionFilter("Parameters.BankBin", 
                    ConditionOperator.SubSet, new List<string> {"Bank1", "Bank2", "Bank3" } )
                },
                new BasicExpression
                {
                    Key = "Parameters.Age",
                    Value = 20,
                    HasPriority = true,
                    Operator = ConditionOperator.GreaterThanOrEqual
                }

            },

            Operator = OperationOperator.And
        }
    },
    ResultExpression = new List<ExpressionGroup>
    {
        new ExpressionGroup
        {
            Criteria = new List<AbstractExpression>
            {
                new OperationExpression
                {
                    Key = "Basket.Products",
                    Filter = new AssignToResultFilter()
                    {
                        ResultSet = new List<ResultProperty>
                        {
                            new ResultProperty {Name = "Discount"},
                            new ResultProperty {Name = "Basket", PickUpFromParameters = "Basket"}
                        },
                        Filter = new ApplyToSumFilter("UnitPrice", "Discount", 20, ApplyOperator.Percent)
                    }
                }
            }
        }
    },
    Operator = OperationOperator.None
};

return expressionMetadata;

```

##Generated Js
```js
function ExpFunc(parametersJson) {

  var p = JSON.parse(parametersJson);
  var result = {
    value: ''
  };

  if ((
      (function() {
        var _pFound = false;

        if (Object.prototype.toString.call(p.Parameters.BankBin) === '[object Array]' 
        && typeof p.Parameters.BankBin[0] == 'object') {
          _.chain(p.Parameters.BankBin).pluck('Parameters.BankBin').each(function(key) {
            if (["Bank1", "Bank2", "Bank3"].indexOf(key) !== -1) {
              _pFound = true;
              return true;
            }

            return false;

          }).value();

        } else {

          _.chain(p.Parameters.BankBin).each(function(key) {
            if (["Bank1", "Bank2", "Bank3"].indexOf(key) !== -1) {
              _pFound = true;
              return true;
            }

            return false;

          }).value();

        }

        return _pFound;
      })()
    ) && (p.Parameters.Age >= 20)) {

    p.Basket.Products = (function() {
      var _pFilterResult = _.chain(p.Basket.Products).filter(function(item) {

        if (([12, 14].indexOf(item.Parameters.BoutiqueId) !== -1) 
        && (["Adidas"].indexOf(item.Parameters.Brand) !== -1)) {
          return true;
        }

        return false;

      }).value();

      return _pFilterResult;
    })();


    try {
      result.Discount = (((_.chain(p.Basket.Products)
        .pluck('UnitPrice')
        .sum()
        .value())) * (20 / 100));
      result.Basket = p.Basket;

    } catch (exception) {
      /*TODO: handle exc.*/
    }
  }

  return JSON.stringify(result);
}
```

##Bugs
If you encounter a bug, performance issue, or malfunction, please add an [Issue](https://github.com/ziyasal/jexpr/issues) with steps on how to reproduce the problem.

##TODO
- Add more tests
- Add more documentation

##License
Code and documentation are available according to the *MIT* License (see [LICENSE](https://github.com/ziyasal/jexpr/blob/master/LICENSE)).
