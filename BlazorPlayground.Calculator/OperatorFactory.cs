namespace BlazorPlayground.Calculator {
    internal static class OperatorFactory {
        public static Operator? GetOperator(char character) => character switch {
            '/' => new DivisionOperator(),
            '÷' => new DivisionOperator(),
            '*' => new MultiplicationOperator(),
            '×' => new MultiplicationOperator(),
            '-' => new SubtractionOperator(),
            '−' => new SubtractionOperator(),
            '+' => new AdditionOperator(),
            _ => null
        };
    }
}
