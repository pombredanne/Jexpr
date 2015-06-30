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
                        new MacroExpression
                        {
                            Key = "Basket.Products",
                            MacroOperator =
                                new TakeFilter {PropertyToVisit = "UnitPrice", Take = 2, Operator = FilterOperator.SumOfMinXItem}
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

                Operator = ExpressionGroupOp.And,
                ReturnType = returnType
            };

            return expressionMetadata;
        }
        public static ExpressionMetadata GetMacroExprDef4MinMax(int num, ExpressionOp op, ReturnTypes returnType, string key, FilterOperator filterOperator)
        {
            AbstractFilter filter = null;

            switch (filterOperator)
            {
                case FilterOperator.Min:
                    filter = new MinFilter { PropertyToVisit = "UnitPrice", Operator = filterOperator };
                    break;
                case FilterOperator.Max:
                    filter = new MaxFilter { PropertyToVisit = "UnitPrice", Operator = filterOperator };
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
                            new MacroExpression {Key = key, Value = num, MacroOperator = filter}
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                MacroOperator = new GroupByFilter{PropertyToVisit = "Parameters.BoutiqueId",Key = "BoutiqueId",GroupSet = "Products",Operator = FilterOperator.GroupBy}
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.BoutiqueId",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<int> {12, 14}
                                            },
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
                                    }
                            },
                            new MacroExpression
                            {
                                Key = "Parameters.BankBin",
                                Value = new List<string> {"Garanti", "Teb", "Finans"},
                                HasPriority = true,
                                MacroOperator =
                                    new ExistsFilter {PropertyToVisit = "Parameters.BankBin", Operator = FilterOperator.Contains}
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
                                MacroOperator = new AssignToResultFilter
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
                                        Operator = FilterOperator.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                   Operator = FilterOperator.Assign
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
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Nike"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
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
                                MacroOperator = new AssignToResultFilter
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
                                        Operator = FilterOperator.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                    Operator = FilterOperator.Assign
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
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
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
                                MacroOperator = new AssignToResultFilter
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
                                        Operator = FilterOperator.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                    Operator = FilterOperator.Assign
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
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Nike"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
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
                                MacroOperator = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    InnerFilter = new AssignTakeToResultFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                         ResultProperty = "Discount",
                                        Operator = FilterOperator.SumOfMinXItem
                                    },
                                    Operator = FilterOperator.Assign
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
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
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
                                MacroOperator = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    InnerFilter = new ApplyExactToSumFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        Amount = 100,
                                        Operator = FilterOperator.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                    Operator = FilterOperator.Assign
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
                                MacroOperator =
                                    new SelectFilter
                                    {
                                        Filters = new List<AbstractFilter>
                                        {
                                            new IndexOfFilter
                                            {
                                                PropertyToVisit = "Parameters.Brand",
                                                Operator = FilterOperator.Contains,
                                                ValueToLookup = new List<string> {"Adidas"}
                                            }
                                        },
                                        Operator = FilterOperator.Select,
                                        AssignTo = "Basket.Products"
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
                                MacroOperator = new AssignToResultFilter
                                {
                                    ResultProperties = new List<ResultProperty>
                                    {
                                        new ResultProperty {Name = "Discount"},
                                        new ResultProperty {Name = "Basket", PropertyToPickUpFromParameters = "Basket"}
                                    },
                                    InnerFilter = new ApplyExactToSumUsingParamtersFilter
                                    {
                                        PropertyToVisit = "UnitPrice",
                                        ParameterName = "CodeDiscountAmount",
                                        Operator = FilterOperator.Sum,
                                        ResultProperty = "Discount",
                                        MultiplierPropertyToVisit = "Quantity"
                                    },
                                    Operator = FilterOperator.Assign
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
                            new MacroExpression
                            {
                                Key = "Basket.Products",
                                Value = new List<int> {testIntParam,12, 14},
                                MacroOperator = new ExistsFilter{PropertyToVisit = "Parameters.BoutiqueId",Operator = FilterOperator.Contains}
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