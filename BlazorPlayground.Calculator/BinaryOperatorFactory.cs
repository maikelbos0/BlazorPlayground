namespace BlazorPlayground.Calculator {
    internal static class BinaryOperatorFactory {
        internal static IBinaryOperator? GetOperator(char character) => character switch {
            '/' => new DivisionOperator(character),
            '÷' => new DivisionOperator(character),
            '*' => new MultiplicationOperator(character),
            '×' => new MultiplicationOperator(character),
            '-' => new SubtractionOperator(character),
            '−' => new SubtractionOperator(character),
            '+' => new AdditionOperator(character),
            _ => null
        };
    }
}
