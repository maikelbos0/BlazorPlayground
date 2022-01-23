namespace BlazorPlayground.Calculator {
    internal static class SymbolFactory {
        internal static ISymbol GetSymbol(char character) => character switch {
            '/' => new DivisionOperator(character),
            '÷' => new DivisionOperator(character),
            '*' => new MultiplicationOperator(character),
            '×' => new MultiplicationOperator(character),
            '-' => new SubtractionOperator(character),
            '−' => new SubtractionOperator(character),
            '+' => new AdditionOperator(character),
            '±' => new NegationOperator(),
            'π' => new Pi(),
            _ => new Character(character)
        };
    }
}
