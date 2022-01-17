namespace BlazorPlayground.Calculator {
    internal class AdditionOperator : Operator {
        public override decimal Invoke(decimal left, decimal right) => left + right;
    }
}
