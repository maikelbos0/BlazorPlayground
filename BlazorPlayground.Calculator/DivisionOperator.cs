namespace BlazorPlayground.Calculator {
    internal class DivisionOperator : Operator {
        public override decimal Invoke(decimal left, decimal right) => left / right;
    }
}
