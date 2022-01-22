using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class CalculationExpressionTests {
        [Fact]
        public void CalculationExpression_TryAppend_Open_Parenthesi_Opens_Group() {
            var expression = new CalculationExpression();

            Assert.True(expression.TryAppend('('));
            Assert.Equal(2, expression.Groups.Count);
        }

        [Fact]
        public void CalculationExpression_TryAppend_Open_Parenthesi_Closes_Group() {
            var expression = new CalculationExpression();
            var group = new SymbolGroup();

            expression.CurrentGroup.Symbols.Add(group);
            expression.Groups.Push(group);

            Assert.True(expression.TryAppend(')'));
            Assert.Single(expression.Groups);
        }

        [Fact]
        public void CalculationExpression_TryAppend_Operator_Creates_Operator() {
            var expression = new CalculationExpression();

            expression.CurrentGroup.Symbols.Add(new LiteralNumber(1));

            Assert.True(expression.TryAppend('*'));
            Assert.Single(expression.Groups);
            Assert.Equal(2, expression.CurrentGroup.Symbols.Count);
            Assert.IsType<MultiplicationOperator>(expression.CurrentGroup.Symbols.Last());
        }

        [Fact]
        public void CalculationExpression_TryAppend_Uses_Subtraction_Operator_Instead_Of_Negation_When_Possible() {
            var expression = new CalculationExpression();

            expression.CurrentGroup.Symbols.Add(new ComposableNumber());

            Assert.True(expression.TryAppend('-'));
            Assert.Single(expression.Groups);
            Assert.Equal(2, expression.CurrentGroup.Symbols.Count);
            Assert.IsType<SubtractionOperator>(expression.CurrentGroup.Symbols.Last());
        }

        [Fact]
        public void CalculationExpression_TryAppend_Digit_Creates_New_ComposableNumber() {
            var expression = new CalculationExpression();

            Assert.True(expression.TryAppend('1'));
            Assert.Single(expression.Groups);
            Assert.Equal('1', Assert.Single(Assert.IsType<ComposableNumber>(Assert.Single(expression.CurrentGroup.Symbols)).Characters));
        }

        [Fact]
        public void CalculationExpression_TryAppend_Adds_Digit_To_Existing_ComposableNumber() {
            var expression = new CalculationExpression();
            var number = new ComposableNumber();

            expression.CurrentGroup.Symbols.Add(number);

            Assert.True(expression.TryAppend('1'));
            Assert.Single(expression.Groups);
            Assert.Single(expression.CurrentGroup.Symbols);
            Assert.Equal('1', Assert.Single(number.Characters));
        }

        [Fact]
        public void CalculationExpression_TryAppend_Does_Nothing_For_Invalid_Character() {
            var expression = new CalculationExpression();

            Assert.False(expression.TryAppend('a'));
            Assert.Single(expression.Groups);
            Assert.Empty(expression.CurrentGroup.Symbols);
        }

        [Fact]
        public void CalculationExpression_TryAppend_Does_Nothing_For_Invalid_Character_With_Existing_ComposableNumber() {
            var expression = new CalculationExpression();
            var number = new ComposableNumber();

            expression.CurrentGroup.Symbols.Add(number);

            Assert.False(expression.TryAppend('a'));
            Assert.Single(expression.Groups);
            Assert.Single(expression.CurrentGroup.Symbols);
            Assert.Empty(number.Characters);
        }

        [Fact]
        public void CalculationExpression_OpenGroup_Adds_New_Group_And_Opens_It() {
            var expression = new CalculationExpression();

            Assert.True(expression.OpenGroup());
            Assert.Equal(2, expression.Groups.Count);
            Assert.Equal(expression.Groups.Peek(), expression.Groups.Last().Symbols.FirstOrDefault());
        }

        [Fact]
        public void CalculationExpression_OpenGroup_Does_Nothing_When_Not_Possible() {
            var expression = new CalculationExpression();

            expression.CurrentGroup.Symbols.Add(new LiteralNumber(0));

            Assert.False(expression.OpenGroup());
            Assert.Single(expression.Groups);
        }

        [Fact]
        public void CalculationExpression_CloseGroup_Closes_Group() {
            var expression = new CalculationExpression();
            var group = new SymbolGroup();

            expression.CurrentGroup.Symbols.Add(group);
            expression.Groups.Push(group);

            Assert.True(expression.CloseGroup());
            Assert.Single(expression.Groups);
        }

        [Fact]
        public void CalculationExpression_CloseGroup_Does_Nothing_For_Root_Group() {
            var expression = new CalculationExpression();

            Assert.False(expression.CloseGroup());
            Assert.Single(expression.Groups);
        }

        [Fact]
        public void CalculationExpression_Clear_Clears_And_Starts_With_New_Group() {
            var expression = new CalculationExpression();
            var group = new SymbolGroup();

            expression.CurrentGroup.Symbols.Add(group);
            expression.Groups.Push(group);
            expression.CurrentGroup.Symbols.Add(new LiteralNumber(0));

            expression.Clear();

            Assert.Single(expression.Groups);
            Assert.Empty(expression.CurrentGroup.Symbols);
        }

        [Fact]
        public void CalculationExpression_Evaluate_Gives_And_Stores_Result() {
            var expression = new CalculationExpression();

            expression.CurrentGroup.Symbols.Add(new LiteralNumber(5.5M));

            Assert.Equal(5.5M, expression.Evaluate());
            Assert.Single(expression.Groups);
            Assert.Equal(5.5M, Assert.IsType<LiteralNumber>(Assert.Single(expression.CurrentGroup.Symbols)).Value);
        }

        [Fact]
        public void CalculationExpression_ToString_Succeeds() {

            var expression = new CalculationExpression();
            var group1 = new SymbolGroup();
            var group2 = new SymbolGroup();

            expression.CurrentGroup.Symbols.Add(new LiteralNumber(5.5M));
            expression.CurrentGroup.Symbols.Add(new AdditionOperator('+'));
            expression.CurrentGroup.Symbols.Add(group1);
            expression.Groups.Push(group1);
            expression.CurrentGroup.Symbols.Add(new LiteralNumber(4.5M));
            expression.CurrentGroup.Symbols.Add(new MultiplicationOperator('*'));
            expression.CurrentGroup.Symbols.Add(new LiteralNumber(3.5M));
            expression.CurrentGroup.Symbols.Add(new DivisionOperator('/'));
            expression.CurrentGroup.Symbols.Add(group2);
            expression.Groups.Push(group2);
            expression.CurrentGroup.Symbols.Add(new LiteralNumber(6.5M));
            expression.Groups.Pop().Close();

            Assert.Equal("5.5 + (4.5 * 3.5 / (6.5)", expression.ToString());
        }
    }
}
