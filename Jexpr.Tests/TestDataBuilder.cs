using System.Collections.Generic;
using Jexpr.Filters;
using Jexpr.Models;

namespace Jexpr.Tests
{
    public class TestDataBuilder
    {
        public static ExpressionMetadata GetExprDefinition()
        {
            ExpressionMetadata metadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new SimpleJexprExpression
                            {
                                Key = "BoutiqueId",
                                Value = 258,
                                Operator = ExpressionOp.Equal
                            }
                            ,
                            new SimpleJexprExpression
                            {
                                Key = "BasketTotal",
                                Value = 200,
                                Operator = ExpressionOp.Equal
                            },
                            new SimpleJexprExpression
                            {
                                Key = "CodeCampaign",
                                Value = 63,
                                Operator = ExpressionOp.LowerThan
                            },
                            new SimpleJexprExpression
                            {
                                Key = "BankBin",
                                Value = "Garanti",
                                Operator = ExpressionOp.Equal
                            },
                            new SimpleJexprExpression
                            {
                                Key = "SavedCreditCard",
                                Value = 1,
                                Operator = ExpressionOp.Equal
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },

                Operator = ExpressionGroupOp.And,
                ReturnType = ReturnTypes.Boolean
            };

            return metadata;
        }


        public static List<ExpressionGroup> GetResultExpression(int num)
        {
            return new List<ExpressionGroup>
            {
                new ExpressionGroup
                {
                    Items = new List<JexprExpression>
                    {
                        new JexprMacroExpression
                        {
                            Key = "Basket.Products",
                            MacroOp =
                                new TakeFilter {PropertyToVisit = "UnitPrice", Take = 2, Op = MacroOp.SumOfMinXItem}
                        }
                    },

                    Operator = ExpressionGroupOp.And
                }
            };
        }

        public static ExpressionMetadata GetBasicExprDef(int num, ExpressionOp op, ReturnTypes returnType, string key)
        {
            var expressionList = new List<JexprExpression>
            {
                new SimpleJexprExpression{Key = key,Value = num,Operator = op}
            };


            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = expressionList,

                        Operator = ExpressionGroupOp.And
                    }
                },

                Operator = ExpressionGroupOp.And,
                ReturnType = returnType
            };

            return expressionMetadata;
        }
        public static ExpressionMetadata GetMacroExprDef4MinMax(int num, ExpressionOp op, ReturnTypes returnType, string key, MacroOp macroOp)
        {
            JexprFilter filter = null;

            switch (macroOp)
            {
                case MacroOp.Min:
                    filter = new MinFilter { PropertyToVisit = "UnitPrice", Op = macroOp };
                    break;
                case MacroOp.Max:
                    filter = new MaxFilter { PropertyToVisit = "UnitPrice", Op = macroOp };
                    break;
            }

            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression {Key = key, Value = num, MacroOp = filter}
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },

                Operator = ExpressionGroupOp.And,
                ReturnType = returnType
            };

            return expressionMetadata;
        }

        public static ExpressionMetadata GetMacroExprDef4GroupBy()
        {
            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = "Basket.Products",
                                MacroOp = new GroupByFilter{PropertyToVisit = "Parameters.BoutiqueId",Key = "BoutiqueId",GroupSet = "Products",Op = MacroOp.GroupBy}
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },
                Operator = ExpressionGroupOp.Return,
                ReturnType = ReturnTypes.JsonString
            };

            return expressionMetadata;
        }

        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenarion1()
        {
            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = "Basket.Products",
                                MacroOp =
                                    new SelectFilter
                                    {
                                        Filters = new List<JexprFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.BoutiqueId",
                                                Op = MacroOp.Contains,
                                                ValueToSearch = new List<int> {12, 14}
                                            },
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Op = MacroOp.Contains,
                                                ValueToSearch = new List<string> {"Adidas"}
                                            }
                                        },
                                        Op = MacroOp.Select,
                                        AssignTo = "Basket.Products"
                                    }
                            },
                            new JexprMacroExpression
                            {
                                Key = "Parameters.BankBin",
                                Value = new List<string> {"Garanti", "Teb", "Finans"},
                                HasPriority = true,
                                MacroOp =
                                    new ExistsFilter {PropertyToVisit = "Parameters.BankBin", Op = MacroOp.Contains}
                            },
                            new SimpleJexprExpression
                            {
                                Key = "Parameters.Age",
                                Value = 20,
                                HasPriority = true,
                                Operator = ExpressionOp.GreaterThenOrEqual
                            }

                        },

                        Operator = ExpressionGroupOp.And
                    }
                },
                ResultExpression = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = "Basket.Products",
                                MacroOp = new SetResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    InnerFilter = new ApplyPercentToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Percent = 20,
                                        Op = MacroOp.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertToVisit = "Quantity"
                                    },
                                   Op = MacroOp.Assign
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return,
                ReturnType = ReturnTypes.JsonString
            };

            return expressionMetadata;
        }

        public static ExpressionMetadata GetInExprDef(int testIntParam, ReturnTypes returnType, string boutiqueid)
        {
            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = "Basket.Products",
                                Value = new List<int> {testIntParam,12, 14},
                                MacroOp = new ExistsFilter{PropertyToVisit = "Parameters.BoutiqueId",Op = MacroOp.Contains}
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },
                Operator = ExpressionGroupOp.Return,
                ReturnType = returnType
            };

            return expressionMetadata;

        }
    }
}