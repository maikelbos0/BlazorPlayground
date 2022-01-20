using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SymbolGroupTests {
        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Empty() {
            var group = new SymbolGroup();
            var literalNumber = new LiteralNumber(0);

            Assert.True(group.Append(literalNumber));
            Assert.Equal(literalNumber, Assert.Single(group.Symbols));
        }

        [Fact]
        public void SymbolGroup_Can_Append_BinaryOperator_When_Empty() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            Assert.True(group.Append(op));            
            Assert.Equal(2, group.Symbols.Count);
            Assert.Equal(op, group.Symbols[1]);
            Assert.Equal(0, Assert.IsType<LiteralNumber>(group.Symbols[0]).Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Last_Symbol_Is_BinaryOperator() {
            var group = new SymbolGroup();
            var literalNumber = new LiteralNumber(0);

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));

            Assert.True(group.Append(literalNumber));
            Assert.Equal(3, group.Symbols.Count);
            Assert.Equal(literalNumber, group.Symbols[2]);
        }

        [Fact]
        public void SymbolGroup_Append_BinaryOperator_Replaces_Last_Symbol_If_BinaryOperator() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));

            Assert.True(group.Append(op));
            Assert.Equal(2, group.Symbols.Count);
            Assert.Equal(op, group.Symbols[1]);
        }

        [Fact]
        public void SymbolGroup_Can_Append_BinaryOperator_When_Last_Symbol_Is_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            group.Symbols.Add(new LiteralNumber(1));

            Assert.True(group.Append(op));
            Assert.Equal(2, group.Symbols.Count);
            Assert.Equal(op, group.Symbols[1]);
        }

        [Fact]
        public void SymbolGroup_Can_Not_Append_EvaluatableSymbol_When_Last_Symbol_Is_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var literalNumber = new LiteralNumber(1);

            group.Symbols.Add(new LiteralNumber(1));

            Assert.False(group.Append(literalNumber));
            Assert.Single(group.Symbols);
        }

        [Fact]
        public void SymbolGroup_Can_Append_UnaryOperator_When_Last_Symbol_Is_BinaryOperator() {
            var group = new SymbolGroup();
            var op = new NegationOperator(new LiteralNumber(1));

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));

            Assert.True(group.Append(op));
            Assert.Equal(3, group.Symbols.Count);
            Assert.Equal(op, group.Symbols[2]);
        }

        [Fact]
        public void SymbolGroup_Append_UnaryOperator_Replaces_Last_Symbol_If_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var number = new LiteralNumber(1);
            var op = new NegationOperator(number);

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(number);

            Assert.True(group.Append(op));
            Assert.Equal(3, group.Symbols.Count);
            Assert.Equal(op, group.Symbols[2]);
        }

        [Fact]
        public void SymbolGroup_Close_Removes_Trailing_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new AdditionOperator('+'));

            group.Close();

            Assert.Equal(3, group.Symbols.Count);
        }

        [Fact]
        public void SymbolGroup_Close_Adds_Zero_When_Empty() {
            var group = new SymbolGroup();

            group.Close();

            Assert.Equal(0, Assert.IsType<LiteralNumber>(Assert.Single(group.Symbols)).Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Empty_Group() {
            var group = new SymbolGroup();

            Assert.Equal(0M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Simple_Expression() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new DivisionOperator('/'));
            group.Symbols.Add(new LiteralNumber(1.6M));

            Assert.Equal(1.75M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Expression_With_Multiple_Same_Precedence_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new DivisionOperator('/'));
            group.Symbols.Add(new LiteralNumber(1.6M));
            group.Symbols.Add(new MultiplicationOperator('*'));
            group.Symbols.Add(new LiteralNumber(3.2M));

            Assert.Equal(5.6M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Expression_With_Multiple_Different_Precedence_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(1.2M));
            group.Symbols.Add(new MultiplicationOperator('*'));
            group.Symbols.Add(new LiteralNumber(3.2M));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(2.8M));

            Assert.Equal(9.44M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_ToString_Succeeds() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(1.2M));
            group.Symbols.Add(new AdditionOperator('*'));

            Assert.Equal("(2.8 + 1.2 *)", group.ToString());
        }
    }
}
