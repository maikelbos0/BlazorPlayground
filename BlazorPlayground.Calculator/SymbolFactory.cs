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
            '²' => new SquareOperator(),
            '√' => new SquareRootOperator(),
            'π' => new Pi(),
            _ => new Character(character)
        };
    }
}
