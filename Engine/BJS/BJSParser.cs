using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BoyJackEngine.BJS
{
    public class BJSParser
    {
        private List<string> _lines;
        private int _currentLine;

        public BJSScript Parse(string scriptContent)
        {
            _lines = new List<string>(scriptContent.Split('\n'));
            _currentLine = 0;

            var script = new BJSScript();
            
            while (_currentLine < _lines.Count)
            {
                string line = _lines[_currentLine].Trim();
                
                if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                {
                    _currentLine++;
                    continue;
                }

                if (line.StartsWith("on "))
                {
                    ParseEventHandler(script, line);
                }
                else if (line.StartsWith("func "))
                {
                    ParseFunction(script, line);
                }
                else
                {
                    // Global variable assignment or statement
                    script.GlobalStatements.Add(ParseStatement(line));
                }
                
                _currentLine++;
            }

            return script;
        }

        private void ParseEventHandler(BJSScript script, string line)
        {
            var match = Regex.Match(line, @"on\s+(\w+):");
            if (match.Success)
            {
                string eventName = match.Groups[1].Value;
                var statements = ParseBlock();
                script.EventHandlers[eventName] = statements;
            }
        }

        private void ParseFunction(BJSScript script, string line)
        {
            var match = Regex.Match(line, @"func\s+(\w+)\((.*?)\):");
            if (match.Success)
            {
                string functionName = match.Groups[1].Value;
                string parameters = match.Groups[2].Value;
                
                var function = new BJSFunction
                {
                    Name = functionName,
                    Parameters = string.IsNullOrEmpty(parameters) ? 
                        new List<string>() : 
                        new List<string>(parameters.Split(',').Select(p => p.Trim()))
                };
                
                function.Statements = ParseBlock();
                script.Functions[functionName] = function;
            }
        }

        private List<BJSStatement> ParseBlock()
        {
            var statements = new List<BJSStatement>();
            _currentLine++;

            while (_currentLine < _lines.Count)
            {
                string line = _lines[_currentLine].Trim();
                
                if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                {
                    _currentLine++;
                    continue;
                }

                // Check if we've reached the end of the block (next event, function, or unindented line)
                if (line.StartsWith("on ") || line.StartsWith("func ") || !_lines[_currentLine].StartsWith(" "))
                {
                    _currentLine--;
                    break;
                }

                statements.Add(ParseStatement(line));
                _currentLine++;
            }

            return statements;
        }

        private BJSStatement ParseStatement(string line)
        {
            line = line.Trim();

            // Assignment
            if (line.Contains("=") && !line.Contains("=="))
            {
                var parts = line.Split('=', 2);
                return new BJSStatement
                {
                    Type = BJSStatementType.Assignment,
                    Variable = parts[0].Trim(),
                    Expression = parts[1].Trim()
                };
            }

            // If statement
            if (line.StartsWith("if "))
            {
                var match = Regex.Match(line, @"if\s+(.+):");
                return new BJSStatement
                {
                    Type = BJSStatementType.If,
                    Condition = match.Groups[1].Value
                };
            }

            // Elif statement
            if (line.StartsWith("elif "))
            {
                var match = Regex.Match(line, @"elif\s+(.+):");
                return new BJSStatement
                {
                    Type = BJSStatementType.ElseIf,
                    Condition = match.Groups[1].Value
                };
            }

            // Function call
            if (line.Contains("(") && line.Contains(")"))
            {
                return new BJSStatement
                {
                    Type = BJSStatementType.FunctionCall,
                    Expression = line
                };
            }

            // Default to expression
            return new BJSStatement
            {
                Type = BJSStatementType.Expression,
                Expression = line
            };
        }
    }
}