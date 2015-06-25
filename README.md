# Jexpr

[![Build status](https://ci.appveyor.com/api/projects/status/vrhmd31v1g86avo8/branch/master?svg=true)](https://ci.appveyor.com/project/ziyasal/jexpr/branch/master)

**Jexpr** is an experimental expression engine that creates javascript code from custom definitions to evaluate expressions. It uses [Jint](https://github.com/sebastienros/jint "Javascript Interpreter for .NET") and [Lodash](https://github.com/lodash/lodash "A JavaScript utility library delivering consistency, modularity, performance, & extras.") internally.

**Sample generated js from following pseudo definition**

```sh
  WITH (`basket`, `profile`, `payment information`) do
            `BoutiqueId` should be one of [12, 14]  
      AND   `Brand` should be one of [Adidas]
      AND   `BankBin` should be one of [Bank1, Bank2, Bank3] 
      AND   `Age` should greater than or equal 20
  RET   
      APPLY 20% TO `Total Basket Price`
```
```js
function ExpFunc(parametersJson) {

    var p = JSON.parse(parametersJson);
    var result = {value: ''};

    if (( ["Bank1", "Bank2", "Bank3"].indexOf(p.Parameters.BankBin) !== -1 ) && (p.Parameters.Age >= 20)) {
        (
            p.Basket.Products = (function () {
                var _pFilterResult = _.chain(p.Basket.Products).filter(function (item) {

                    if (( [12, 14].indexOf(item.Parameters.BoutiqueId) !== -1 ) && ( ["Adidas"].indexOf(item.Parameters.Brand) !== -1 )) {
                        return true;
                    }

                    return false;

                }).value();

                return _pFilterResult;
            })()
        )
    }

    result.Discount = ( (
        (function () {
            var _pTotal = 0;
            _.chain(p.Basket.Products).each(function (item) {
                var _pUnitPrice = item.UnitPrice;
                var _pQuantity = item.Quantity;

                _pTotal += (_pUnitPrice * _pQuantity);
            }).value();

            return _pTotal;
        })()
    ) * (20 / 100) )
    
    result.Basket = p.Basket

    return JSON.stringify(result);
}
```
