using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BoyJackEngine.BJS
{
    public class BJSExecutor
    {
        private Dictionary<string, BJSVariable> _variables;
        private BJSBuiltins _builtins;
        private BJSScript _currentScript;

        public BJSExecutor(BJSBuiltins builtins)
        {
            _variables = new Dictionary<string, BJSVariable>();
            _builtins = builtins;
        }

        public void ExecuteScript(BJSScript script)
        {
            _currentScript = script;
            
            // Execute global statements
            foreach (var statement in script.GlobalStatements)
            {
                ExecuteStatement(statement);
            }
        }

        public void ExecuteEvent(string eventName)
        {
            if (_currentScript?.EventHandlers.ContainsKey(eventName) == true)
            {
                foreach (var statement in _currentScript.EventHandlers[eventName])
                {
                    ExecuteStatement(statement);
                }
            }
        }

        private void ExecuteStatement(BJSStatement statement)
        {
            try
            {
                switch (statement.Type)
                {
                    case BJSStatementType.Assignment:
                        ExecuteAssignment(statement);
                        break;
                    case BJSStatementType.FunctionCall:
                        ExecuteFunctionCall(statement.Expression);
                        break;
                    case BJSStatementType.If:
                        ExecuteIf(statement);
                        break;
                    case BJSStatementType.Expression:
                        EvaluateExpression(statement.Expression);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BJS Runtime Error: {ex.Message}");
            }
        }

        private void ExecuteAssignment(BJSStatement statement)
        {
            var value = EvaluateExpression(statement.Expression);
            _variables[statement.Variable] = new BJSVariable(value);
        }

        private void ExecuteIf(BJSStatement statement)
        {
            var condition = EvaluateExpression(statement.Condition);
            var conditionVar = new BJSVariable(condition);
            
            if (conditionVar.AsBoolean())
            {
                foreach (var stmt in statement.ThenStatements)
                {
                    ExecuteStatement(stmt);
                }
            }
            else
            {
                foreach (var stmt in statement.ElseStatements)
                {
                    ExecuteStatement(stmt);
                }
            }
        }

        private object ExecuteFunctionCall(string expression)
        {
            var match = Regex.Match(expression, @"(\w+)\((.*?)\)");
            if (match.Success)
            {
                string functionName = match.Groups[1].Value;
                string argsString = match.Groups[2].Value;
                
                var args = new List<object>();
                if (!string.IsNullOrEmpty(argsString))
                {
                    var argParts = argsString.Split(',');
                    foreach (var arg in argParts)
                    {
                        args.Add(EvaluateExpression(arg.Trim()));
                    }
                }

                // Check if it's a built-in function
                if (_builtins.HasFunction(functionName))
                {
                    return _builtins.CallFunction(functionName, args.ToArray());
                }

                // Check if it's a user-defined function
                if (_currentScript.Functions.ContainsKey(functionName))
                {
                    return ExecuteUserFunction(functionName, args);
                }
            }

            return null;
        }

        private object ExecuteUserFunction(string functionName, List<object> args)
        {
            var function = _currentScript.Functions[functionName];
            
            // Create new scope for function parameters
            var oldVars = new Dictionary<string, BJSVariable>(_variables);
            
            // Set parameters
            for (int i = 0; i < function.Parameters.Count && i < args.Count; i++)
            {
                _variables[function.Parameters[i]] = new BJSVariable(args[i]);
            }

            // Execute function body
            foreach (var statement in function.Statements)
            {
                ExecuteStatement(statement);
            }

            // Restore scope (simplified - in real implementation you'd handle return values)
            _variables = oldVars;
            
            return null;
        }

        private object EvaluateExpression(string expression)
        {
            expression = expression.Trim();

            // String literal
            if (expression.StartsWith("\"") && expression.EndsWith("\""))
            {
                return expression.Substring(1, expression.Length - 2);
            }

            // Number literal
            if (float.TryParse(expression, out float number))
            {
                return number;
            }

            // Boolean literal
            if (expression == "true") return true;
            if (expression == "false") return false;

            // Variable reference
            if (_variables.ContainsKey(expression))
            {
                return _variables[expression].Value;
            }

            // Property access (e.g., player.x)
            if (expression.Contains("."))
            {
                var parts = expression.Split('.');
                if (_variables.ContainsKey(parts[0]))
                {
                    // Simplified property access
                    return GetProperty(parts[0], parts[1]);
                }
            }

            // Function call
            if (expression.Contains("(") && expression.Contains(")"))
            {
                return ExecuteFunctionCall(expression);
            }

            // Comparison operators
            if (expression.Contains("=="))
            {
                var parts = expression.Split(new[] { "==" }, StringSplitOptions.None);
                var left = EvaluateExpression(parts[0].Trim());
                var right = EvaluateExpression(parts[1].Trim());
                return Equals(left, right);
            }

            // Arithmetic operations (simplified)
            if (expression.Contains("+"))
            {
                var parts = expression.Split('+');
                if (parts.Length == 2)
                {
                    var left = EvaluateExpression(parts[0].Trim());
                    var right = EvaluateExpression(parts[1].Trim());
                    
                    if (left is string || right is string)
                        return left.ToString() + right.ToString();
                    
                    return Convert.ToSingle(left) + Convert.ToSingle(right);
                }
            }

            return expression;
        }

        private object GetProperty(string objectName, string propertyName)
        {
            // Simplified property access - in a real implementation this would be more robust
            if (!_variables.ContainsKey(objectName))
            {
                _variables[objectName] = new BJSVariable(new Dictionary<string, object>());
            }

            var objVar = _variables[objectName];
            if (objVar.Value is Dictionary<string, object> dict)
            {
                return dict.ContainsKey(propertyName) ? dict[propertyName] : 0.0f;
            }

            return 0.0f;
        }

        public void SetVariable(string name, object value)
        {
            _variables[name] = new BJSVariable(value);
        }

        public BJSVariable GetVariable(string name)
        {
            return _variables.ContainsKey(name) ? _variables[name] : new BJSVariable(null);
        }
    }
}