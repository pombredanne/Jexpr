﻿using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Tests
{
    public class TestDataBuilder
    {
        public static ExpressionDefinition GetExprDefinition()
        {
            ExpressionDefinition definition = new ExpressionDefinition
            {
                Groups = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprExpression
                            {
                                Key = "BoutiqueId",
                                Value = 258,
                                Operator = ExpressionOp.Eq
                            }
                            ,
                            new JexprExpression
                            {
                                Key = "BasketTotal",
                                Value = 200,
                                Operator = ExpressionOp.Eq
                            },
                            new JexprExpression
                            {
                                Key = "CodeCampaign",
                                Value = 63,
                                Operator = ExpressionOp.Lt
                            },
                            new JexprExpression
                            {
                                Key = "BankBin",
                                Value = "Garanti",
                                Operator = ExpressionOp.Eq
                            },
                            new JexprExpression
                            {
                                Key = "SavedCreditCard",
                                Value = 1,
                                Operator = ExpressionOp.Eq
                            }
                        },

                        Op = ExpressionGroupOp.And
                    }
                },

                Op = ExpressionGroupOp.And,
                ReturnType = ReturnTypes.Boolean
            };

            return definition;
        }

        public static ExpressionDefinition GetExprDefinition4Basket()
        {
            ExpressionDefinition expressionDefinition = new ExpressionDefinition
            {
                Groups = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = "Basket.Items",
                                Value = 10,
                                Operator = ExpressionOp.Gte,
                                MacroOp = new FilterMacroOpDefinition
                                {
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

            return expressionDefinition;
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
                            Key = "Basket.Items",
                            MacroOp = new FilterMacroOpDefinition
                            {
                                PropertyToVisit = "Product.Price",
                                Take = 2,
                                Op = MacroOp.SumOfMinXItem
                            }

                        }
                    },

                    Op = ExpressionGroupOp.And
                }
            };
        }

        public static ExpressionDefinition GetBasicExprDef(int num, ExpressionOp op, ReturnTypes returnType, string key)
        {
            var expressionList = new List<JexprExpression>
            {
                new JexprExpression
                {
                    Key = key,
                    Value = num,
                    Operator = op
                }
            };


            ExpressionDefinition expressionDefinition = new ExpressionDefinition
            {
                Groups = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = expressionList,

                        Op = ExpressionGroupOp.And
                    }
                },

                Op = ExpressionGroupOp.And,
                ReturnType = returnType
            };

            return expressionDefinition;
        }
        public static ExpressionDefinition GetMacroExprDef4MinMax(int num, ExpressionOp op, ReturnTypes returnType, string key, MacroOp macroOp)
        {
            ExpressionDefinition expressionDefinition = new ExpressionDefinition
            {
                Groups = new List<ExpressionGroup>
                {
                    new ExpressionGroup
                    {
                        Items = new List<JexprExpression>
                        {
                            new JexprMacroExpression
                            {
                                Key = key,
                                Value = num,
                                Operator = op,
                                MacroOp = new MacroOpDefinition
                                {
                                    PropertyToVisit ="Product.Price",
                                    Op = macroOp
                                }

                            }
                        },

                        Op = ExpressionGroupOp.And
                    }
                },

                Op = ExpressionGroupOp.And,
                ReturnType = ReturnTypes.Boolean
            };

            return expressionDefinition;
        }
    }
}