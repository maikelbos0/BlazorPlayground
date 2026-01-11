using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator;

public class CalculationExpression {
    public CalculationExpression() {
        Groups = new();
        Groups.Push(new());
    }

    internal Stack<SymbolGroup> Groups { get; }
    internal SymbolGroup CurrentGroup => Groups.Peek();

    public bool TryAppend(char character) {
        if (character == '(') {
            // TODO allow group opening with active evaluatable?
            return OpenGroup();
        }
        else if (character == ')') {
            return CloseGroup();
        }
        else if (character == '⌫') {
            if (CurrentGroup.Symbols.LastOrDefault() is SymbolGroup group) {
                Groups.Push(group);
                return true;
            }
            else if (Groups.Count > 1 && CurrentGroup.Symbols.Count == 0) {
                Groups.Pop();
                CurrentGroup.Symbols.RemoveAt(CurrentGroup.Symbols.Count - 1);
                return true;
            }
        }

        var symbol = SymbolFactory.GetSymbol(character);

        return CurrentGroup.TryAppend(symbol);
    }

    // TODO inline these
    internal bool OpenGroup() {
        var group = new SymbolGroup();
        var success = CurrentGroup.TryAppend(group);

        if (success) {
            Groups.Push(group);
        }

        return success;
    }

    internal bool CloseGroup() {
        if (Groups.Count == 1) {
            return false;
        }

        Groups.Pop().Close();

        return true;
    }

    public void Clear() {
        Groups.Clear();
        Groups.Push(new());
    }

    public decimal Evaluate() {
        while (CloseGroup()) { }

        var value = Groups.Pop().Evaluate();

        Groups.Push(new());
        CurrentGroup.TryAppend(new LiteralNumber(value));

        return value;
    }

    override public string ToString() {
        var value = Groups.Last().ToString();

        return value.Substring(1, value.Length - 1 - Groups.Count);
    }
}
