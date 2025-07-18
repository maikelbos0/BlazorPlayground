namespace BlazorPlayground.StateManagement.Tests;

public class StateTests {
    [Fact]
    public void Constructor() {
        var subject = new State<int>(42);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void ConvertToT() {
        var subject = new State<int>(42);

        int value = subject;

        Assert.Equal(42, value);
    }

    [Fact]
    public void ConvertFromT() {
        State<int> subject = 42;

        Assert.Equal(42, subject.Value);
    }
}
