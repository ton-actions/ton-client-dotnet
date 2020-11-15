﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ch1seL.TonNet.ClientGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ch1seL.TonNet.ClientGenerator.Helpers
{
    internal static class ModulesClassHelpers
    {
        public static MemberDeclarationSyntax GetMethodDeclaration(Module module, Function function, bool withBody)
        {
            string callBackType = null;

            var responseType = function.Result.GetMethodReturnType();
            var responseDeclaration = responseType == null ? "Task" : $"Task<{responseType}>";

            var parameters = function.Params
                .Where(p => !p.IsClientContextParam())
                .Select(param => GetMethodParameter(module, param, out callBackType))
                .ToArray();

            var modifiers = new List<SyntaxToken> {Token(SyntaxKind.PublicKeyword)};
            if (withBody) modifiers.Add(Token(SyntaxKind.AsyncKeyword));

            MethodDeclarationSyntax method =
                MethodDeclaration(ParseTypeName(responseDeclaration), NamingConventions.Formatter(function.Name))
                    .AddParameterListParameters(parameters)
                    .AddParameterListParameters(Parameter(Identifier("cancellationToken"))
                        .WithType(IdentifierName(nameof(CancellationToken)))
                        .WithDefault(EqualsValueClause(IdentifierName("default"))))
                    .AddModifiers(modifiers.ToArray());

            if (withBody)
            {
                var arguments = new List<ArgumentSyntax>
                {
                    Argument(IdentifierName($"\"{module.Name}.{function.Name}\""))
                };
                arguments.AddRange(
                    parameters
                        .Select(p => Argument(IdentifierName(p.Identifier.Text))));
                arguments.Add(Argument(IdentifierName("cancellationToken")));

                var genericParametersDeclaration =
                    StringUtils.GetGenericParametersDeclaration(parameters.FirstOrDefault()?.Type?.ToString(), responseType, callBackType);

                AwaitExpressionSyntax awaitExpression = AwaitExpression(
                    InvocationExpression(IdentifierName($"_tonClientAdapter.Request{genericParametersDeclaration}"))
                        .AddArgumentListArguments(arguments.ToArray()));


                StatementSyntax ex = responseType == null
                    ? (StatementSyntax) ExpressionStatement(awaitExpression)
                    : ReturnStatement(awaitExpression);
                BlockSyntax blockSyntax = Block(ex);
                return method.WithBody(blockSyntax);
            }

            return method.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }

        public static ParameterSyntax GetMethodParameter(Module module, Param param, out string callBackType)
        {
            GenericArg genericArg = param.GenericArgs?[0];
            string typeName;

            switch (genericArg?.RefName)
            {
                case GenericRefNames.Request when string.Equals(module.Name, "net", StringComparison.OrdinalIgnoreCase):
                    callBackType = "object";
                    typeName = $"Action<{callBackType}, int>";
                    break;
                case GenericRefNames.Request:
                    callBackType = NamingConventions.EventFormatter(module.Name);
                    typeName = $"Action<{callBackType}, int>";
                    break;
                default:
                    callBackType = null;
                    switch (param.Type)
                    {
                        case ParamType.Ref:
                            typeName = NamingConventions.Formatter(param.RefName);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
            }

            return Parameter(Identifier(StringUtils.EscapeReserved(param.Name.GetEnumMemberValueOrString())))
                .WithType(IdentifierName(typeName));
        }

        public static NamespaceDeclarationSyntax CreateTonModuleClass(string unitName, Module module)
        {
            StatementSyntax statementSyntax = ParseStatement("_tonClientAdapter = tonClientAdapter;");

            VariableDeclarationSyntax variableDeclaration = VariableDeclaration(ParseTypeName("ITonClientAdapter"))
                .AddVariables(VariableDeclarator("_tonClientAdapter"));
            FieldDeclarationSyntax fieldDeclaration = FieldDeclaration(variableDeclaration)
                .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword));

            ConstructorDeclarationSyntax constructorDeclaration = ConstructorDeclaration(unitName)
                .AddParameterListParameters(
                    Parameter(Identifier("tonClientAdapter")).WithType(IdentifierName("ITonClientAdapter")))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithBody(Block(statementSyntax));

            var methods = module
                .Functions
                .Select(f => GetMethodDeclaration(module, f, true))
                .ToArray();

            ClassDeclarationSyntax item = ClassDeclaration(unitName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(fieldDeclaration)
                .AddMembers(constructorDeclaration)
                .AddMembers(methods);

            return NamespaceDeclaration(IdentifierName(Generator.NameSpace))
                .AddMembers(item);
        }

        public static NamespaceDeclarationSyntax CreateTonModuleInterface(string unitName, Module module)
        {
            var methods = module
                .Functions
                .Select(function => GetMethodDeclaration(module, function, false))
                .ToArray();

            InterfaceDeclarationSyntax item = InterfaceDeclaration($"I{unitName}")
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddMembers(methods);

            return NamespaceDeclaration(IdentifierName(Generator.NameSpace))
                .AddMembers(item);
        }
    }
}