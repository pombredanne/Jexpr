# Jexpr

**WARNING : ** _It requires a lot of code refactoring to use in production__


[![Build status](https://ci.appveyor.com/api/projects/status/vrhmd31v1g86avo8/branch/master?svg=true)](https://ci.appveyor.com/project/ziyasal/jexpr/branch/master)

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
        "UnitPrice": 166,
        "Quantity": 151,
        "Parameters": {
          "BoutiqueId": 12,
          "Brand": "Adidas"
        }
      },
      {
        "UnitPrice": 239,
        "Quantity": 58,
        "Parameters": {
          "BoutiqueId": 12,
          "Brand": "Nike"
        }
      },
      {
        "UnitPrice": 139,
        "Quantity": 207,
        "Parameters": {
          "BoutiqueId": 18,
          "Brand": "Nike"
        }
      }
    ]
  },
  "Parameters": {
    "BankBin": "Bank1",
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
                                    ConditionOperator.Contains, 
                                    new List<int> {12, 14}),
                                new ConditionFilter("Parameters.Brand", 
                                    ConditionOperator.Contains, 
                                    new List<string> {"Adidas"})
                            }
                        }
                },
                new OperationExpression
                {
                    Key = "Parameters.BankBin",
                    HasPriority = true,
                    Filter =new ConditionFilter("Parameters.BankBin", 
                            ConditionOperator.SubSet,
                            new List<string> {"Bank1", "Bank2", "Bank3"} )
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
                            new ResultProperty {Name = "Basket", 
                                PickUpFromParameters = "Basket"}
                        },
                        Filter = new ApplyToSumFilter("UnitPrice", "Discount", 20, 
                                 ApplyOperator.Percent)
                    }
                }
            }
        }
    },
    Operator = OperationOperator.None
};
```

##Generated Js
```js
function ExpFunc(parametersJson) {

    var p = JSON.parse(parametersJson);
    var result = {value: ''};

    if ((
            (function () {
                var _pFound = false;
                _.chain(p.Parameters.BankBin).pluck('Parameters.BankBin').each(function (key) {
                    if (["Garanti", "Teb", "Finans"].indexOf(key) !== -1) {
                        _pFound = true;
                        return true;
                    }

                    return false;

                }).value();

                return _pFound;
            })()
        ) && (p.Parameters.Age >= 20)) {
        (
            p.Basket.Products = (function () {
                var _pFilterResult = _.chain(p.Basket.Products).filter(function (item) {

                    if (( [12, 14].indexOf(item.Parameters.BoutiqueId) !== -1 ) 
                      && ( ["Adidas"].indexOf(item.Parameters.Brand) !== -1 )) {
                        return true;
                    }

                    return false;

                }).value();

                return _pFilterResult;
            })()
        )
    }


    result.Discount = ( (( _.chain(p.Basket.Products)
        .pluck('TotalPrice')
        .sum()
        .value() )) * (20 / 100) )
    result.Basket = p.Basket


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
