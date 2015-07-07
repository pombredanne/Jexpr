using System;
using System.Collections.Generic;
using Jexpr.Filters;
using Jexpr.Models;
using Jexpr.Operators;
using Jexpr.Tests.Models;
using Ploeh.AutoFixture;

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
                        Criteria = new List<AbstractExpression>
                        {
                            new BasicExpression
                            {
                                Key = "BoutiqueId",
                                Value = 258,
                                Operator = ConditionOperator.Equal
                            }
                            ,
                            new BasicExpression
                            {
                                Key = "BasketTotal",
                                Value = 200,
                                Operator = ConditionOperator.Equal
                            },
                            new BasicExpression
                            {
                                Key = "CodeCampaign",
                                Value = 63,
                                Operator = ConditionOperator.LowerThan
                            },
                            new BasicExpression
                            {
                                Key = "BankBin",
                                Value = "Garanti",
                                Operator = ConditionOperator.Equal
                            },
                            new BasicExpression
                            {
                                Key = "SavedCreditCard",
                                Value = 1,
                                Operator = ConditionOperator.Equal
                            }
                        },

                        Operator = OperationOperator.And
                    }
                },

                Operator = OperationOperator.And
            };

            return metadata;
        }
        public static List<ExpressionGroup> GetResultExpression(int num)
        {
            return new List<ExpressionGroup>
            {
                new ExpressionGroup
                {
                    Criteria = new List<AbstractExpression>
                    {
                        new OperationExpression
                        {
                            Key = "Basket.Products",
                            Filter = new AssignSumOfXItemToResultFilter("UnitPrice", SortDirection.Descending, 2, String.Empty)
                        }
                    },

                    Operator = OperationOperator.And
                }
            };
        }
        public static ExpressionMetadata GetBasicExprDef(int num, ConditionOperator op, string key)
        {
            var expressionList = new List<AbstractExpression>
            {
                new BasicExpression{Key = key,Value = num,Operator = op}
            };


            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Criteria = expressionList,

                        Operator = OperationOperator.And
                    }
                },

                Operator = OperationOperator.And
            };

            return expressionMetadata;
        }
        public static ExpressionMetadata GetMacroExprDef4MinMax(int num, ConditionOperator op, string key, SortDirection sortDirection)
        {
            AbstractFilter filter = sortDirection == SortDirection.Ascending ? new FunctionFilter("UnitPrice", FunctionOperator.Max) : new FunctionFilter("UnitPrice", FunctionOperator.Min);

            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Criteria = new List<AbstractExpression>
                        {
                            new OperationExpression {Key = key, Value = num, Filter = filter}
                        },

                        Operator = OperationOperator.And
                    }
                },

                Operator = OperationOperator.And
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
                        Criteria = new List<AbstractExpression>
                        {
                            new OperationExpression
                            {
                                Key = "Basket.Products",
                                Filter = new GroupByFilter (propertyToVisit: "Parameters.BoutiqueId", key: "BoutiqueId", groupSet: "Products")
                            }
                        },

                        Operator = OperationOperator.And
                    }
                },
                Operator = OperationOperator.None
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
                        Criteria = new List<AbstractExpression>
                        {
                            new OperationExpression
                            {
                                Key = "Basket.Products",
                                Filter = new SelectFilter
                                {
                                        Conditions = new List<AbstractFilter>
                                        {
                                            new ConditionFilter("Parameters.BoutiqueId", ConditionOperator.Contains, new List<int> {12, 14}),
                                            new ConditionFilter("Parameters.Brand", ConditionOperator.Contains, new List<string> {"Adidas"})
                                        }
                                    }
                            },
                            new OperationExpression
                            {
                                Key = "Parameters.BankBin",
                                HasPriority = true,
                                Filter =new ConditionFilter("Parameters.BankBin", ConditionOperator.SubSet, new List<string> {"Garanti", "Teb", "Finans"} )
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
        }

        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenarion3()
        {
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
                                        new ConditionFilter("Parameters.Brand", ConditionOperator.Contains,
                                            new List<string> {"Nike"})
                                    }
                                }
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
                                Filter = new AssignToResultFilter
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
        }

        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenarion2()
        {
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
                                            new ConditionFilter("Parameters.Brand", ConditionOperator.Contains, new List<string> {"Adidas"})
                                        }
                                    }
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
                                Filter = new AssignToResultFilter
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
        }

        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenario4()
        {
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
                                Filter =
                                    new SelectFilter
                                    {
                                        Conditions = new List<AbstractFilter>
                                        {
                                            new ConditionFilter("Parameters.Brand", ConditionOperator.Contains, new List<string> {"Nike"})
                                        }
                                    }
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
                                Filter = new AssignToResultFilter
                                {
                                    ResultSet = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PickUpFromParameters = "Basket"}
                                    },
                                    Filter = new AssignSumOfXItemToResultFilter("UnitPrice",SortDirection.Descending, 1, "Discount")
                                }
                            }
                        }
                    }
                },
                Operator = OperationOperator.None
            };

            return expressionMetadata;
        }


        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenario_Exact()
        {
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
                                            new ConditionFilter("Parameters.Brand", ConditionOperator.Contains, new List<string> {"Adidas"})
                                        }
                                    }
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
                                Filter = new AssignToResultFilter
                                {
                                    ResultSet = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyToSumFilter("UnitPrice", "Discount", 100, ApplyOperator.Exact)
                                }
                            }
                        }
                    }
                },
                Operator = OperationOperator.None
            };

            return expressionMetadata;
        }

        public static ExpressionMetadata GetMacroExprMetadata4ComplexScenario_Exact_Using_Parameter()
        {
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
                                        new ConditionFilter("Parameters.Brand", ConditionOperator.Contains,
                                            new List<string> {"Adidas"})
                                    }
                                }
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
                                Filter = new AssignToResultFilter
                                {
                                    ResultSet = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyExactToSumUsingParamtersFilter("UnitPrice", "CodeDiscountAmount", "Discount")
                                }
                            }
                        }
                    }
                },
                Operator = OperationOperator.None
            };

            return expressionMetadata;
        }

        public static ExpressionMetadata GetInExprDef(int testIntParam, string boutiqueid)
        {
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
                                Filter = new ConditionFilter("Parameters.BoutiqueId", ConditionOperator.SubSet, new List<int> {testIntParam, 12, 14})
                            }
                        },

                        Operator = OperationOperator.And
                    }
                },
                Operator = OperationOperator.None
            };

            return expressionMetadata;

        }

        public static Dictionary<string, object> GetPromotionParameters()
        {
            IFixture fixture = new Fixture();

            TestBasket testBasket = fixture.Create<TestBasket>();

            testBasket.Products[0].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[1].Parameters.Add("BoutiqueId", 12);
            testBasket.Products[2].Parameters.Add("BoutiqueId", 18);

            testBasket.Products[0].Parameters.Add("Id", 1);
            testBasket.Products[1].Parameters.Add("Id", 2);
            testBasket.Products[2].Parameters.Add("Id", 3);

            testBasket.Products[0].Parameters.Add("Brand", "Nike");
            testBasket.Products[1].Parameters.Add("Brand", "Adidas");
            testBasket.Products[2].Parameters.Add("Brand", "Adidas");


            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Basket", testBasket},
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            return parameters;
        }
    }
}