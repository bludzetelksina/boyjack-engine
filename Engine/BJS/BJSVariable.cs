using System;
using System.Collections.Generic;

namespace BoyJackEngine.BJS
{
    public class BJSVariable
    {
        public object Value { get; set; }
        public BJSVariableType Type { get; set; }

        public BJSVariable(object value)
        {
            Value = value;
            Type = DetermineType(value);
        }

        private BJSVariableType DetermineType(object value)
        {
            return value switch
            {
                null => BJSVariableType.Null,
                bool => BJSVariableType.Boolean,
                int or float or double => BJSVariableType.Number,
                string => BJSVariableType.String,
                Dictionary<string, BJSVariable> => BJSVariableType.Object,
                _ => BJSVariableType.Object
            };
        }

        public float AsNumber()
        {
            return Type switch
            {
                BJSVariableType.Number => Convert.ToSingle(Value),
                BJSVariableType.String when float.TryParse(Value.ToString(), out float result) => result,
                BJSVariableType.Boolean => (bool)Value ? 1.0f : 0.0f,
                _ => 0.0f
            };
        }

        public bool AsBoolean()
        {
            return Type switch
            {
                BJSVariableType.Boolean => (bool)Value,
                BJSVariableType.Number => AsNumber() != 0.0f,
                BJSVariableType.String => !string.IsNullOrEmpty(Value.ToString()),
                BJSVariableType.Null => false,
                _ => true
            };
        }

        public string AsString()
        {
            return Value?.ToString() ?? "";
        }

        public override string ToString() => AsString();
    }

    public enum BJSVariableType
    {
        Null,
        Boolean,
        Number,
        String,
        Object
    }
}