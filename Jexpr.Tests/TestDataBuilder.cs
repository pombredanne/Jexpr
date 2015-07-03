using System.Collections.Generic;
using Jexpr.Filters;
using Jexpr.Models;
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
                        Items = new List<JexprExpression>
                        {
                            new BasicExpression
                            {
                                Key = "BoutiqueId",
                                Value = 258,
                                Operator = ExpressionOp.Equal
                            }
                            ,
                            new BasicExpression
                            {
                                Key = "BasketTotal",
                                Value = 200,
                                Operator = ExpressionOp.Equal
                            },
                            new BasicExpression
                            {
                                Key = "CodeCampaign",
                                Value = 63,
                                Operator = ExpressionOp.LowerThan
                            },
                            new BasicExpression
                            {
                                Key = "BankBin",
                                Value = "Garanti",
                                Operator = ExpressionOp.Equal
                            },
                            new BasicExpression
                            {
                                Key = "SavedCreditCard",
                                Value = 1,
                                Operator = ExpressionOp.Equal
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },

                Operator = ExpressionGroupOp.And
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
                        new MacroExpression
                        {
                            Key = "Basket.Products",
                            Filter =new AssignTakeSumOfMinXItemToResultFilter {PropertyToVisit = "UnitPrice", Take = 2}
                        }
                    },

                    Operator = ExpressionGroupOp.And
                }
            };
        }
        public static ExpressionMetadata GetBasicExprDef(int num, ExpressionOp op, string key)
        {
            var expressionList = new List<JexprExpression>
            {
                new BasicExpression{Key = key,Value = num,Operator = op}
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

                Operator = ExpressionGroupOp.And
            };

            return expressionMetadata;
        }
        public static ExpressionMetadata GetMacroExprDef4MinMax(int num, ExpressionOp op, string key, bool max)
        {
            AbstractFilter filter;

            if (max)
            {
                filter = new MaxFilter { PropertyToVisit = "UnitPrice" };
            }
            else
            {
                filter = new MinFilter { PropertyToVisit = "UnitPrice" };
            }

            ExpressionMetadata expressionMetadata = new ExpressionMetadata
            {
                Items = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression {Key = key, Value = num, Filter = filter}
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },

                Operator = ExpressionGroupOp.And
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new GroupByFilter{PropertyToVisit = "Parameters.BoutiqueId",Key = "BoutiqueId",GroupSet = "Products"}
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.BoutiqueId",
                                                ValueToLookup = new List<int> {12, 14}
                                            },
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        }
                                    }
                            },
                            new MacroExpression
                            {
                                Key = "Parameters.BankBin",
                                HasPriority = true,
                                Filter =new ExistsFilter {PropertyToVisit = "Parameters.BankBin",  ValueToLookup = new List<string> {"Garanti", "Teb", "Finans"}}
                            },
                            new BasicExpression
                            {
                                Key = "Parameters.Age",
                                Value = 20,
                                HasPriority = true,
                                Operator = ExpressionOp.GreaterThanOrEqual
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyPercentToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Percent = 20,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    }
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Nike"}
                                            }
                                        }
                                    }
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyPercentToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Percent = 20,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    }
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        }
                                    }
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyPercentToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Percent = 20,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Nike"}
                                            }
                                        }
                                    }
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new AssignTakeSumOfMinXItemToResultFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                         ResultProperty = "Discount",
                                    },
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        }
                                    }
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyExactToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Amount = 100,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        }
                                    }
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    Filter = new ApplyExactToSumUsingParamtersFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        ParameterName = "CodeDiscountAmount",
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    }
                                }
                            }
                        }
                    }
                },
                Operator = ExpressionGroupOp.Return
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
                        Items = new List<JexprExpression>
                        {
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Filter = new ExistsFilter {PropertyToVisit = "Parameters.BoutiqueId" ,ValueToLookup = new List<int> {testIntParam, 12, 14}}
                            }
                        },

                        Operator = ExpressionGroupOp.And
                    }
                },
                Operator = ExpressionGroupOp.Return
            };

            return expressionMetadata;

        }

        public static Dictionary<string, object> GetPromotionParameters()
        {
            IFixture fixture = new Fixture();

            Basket basket = fixture.Create<Basket>();

            basket.Products[0].Parameters.Add("BoutiqueId", 12);
            basket.Products[1].Parameters.Add("BoutiqueId", 12);
            basket.Products[2].Parameters.Add("BoutiqueId", 18);

            basket.Products[0].Parameters.Add("Id", 1);
            basket.Products[1].Parameters.Add("Id", 2);
            basket.Products[2].Parameters.Add("Id", 3);

            basket.Products[0].Parameters.Add("Brand", "Nike");
            basket.Products[1].Parameters.Add("Brand", "Adidas");
            basket.Products[2].Parameters.Add("Brand", "Adidas");


            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"Basket", basket},
                {"Parameters", new Dictionary<string, object> {{"BankBin", "Garanti"}, {"Age", 20}}}
            };

            return parameters;
        }
    }
}