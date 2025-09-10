using System.Collections.Generic;

namespace BoyJackEngine.BJS
{
    public class BJSScript
    {
        public Dictionary<string, List<BJSStatement>> EventHandlers { get; set; }
        public Dictionary<string, BJSFunction> Functions { get; set; }
        public List<BJSStatement> GlobalStatements { get; set; }

        public BJSScript()
        {
            EventHandlers = new Dictionary<string, List<BJSStatement>>();
            Functions = new Dictionary<string, BJSFunction>();
            GlobalStatements = new List<BJSStatement>();
        }
    }

    public class BJSFunction
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<BJSStatement> Statements { get; set; }

        public BJSFunction()
        {
            Parameters = new List<string>();
            Statements = new List<BJSStatement>();
        }
    }

    public class BJSStatement
    {
        public BJSStatementType Type { get; set; }
        public string Variable { get; set; }
        public string Expression { get; set; }
        public string Condition { get; set; }
        public List<BJSStatement> ThenStatements { get; set; }
        public List<BJSStatement> ElseStatements { get; set; }

        public BJSStatement()
        {
            ThenStatements = new List<BJSStatement>();
            ElseStatements = new List<BJSStatement>();
        }
    }

    public enum BJSStatementType
    {
        Assignment,
        Expression,
        FunctionCall,
        If,
        ElseIf,
        Else
    }
}