namespace BlazorPlayground.StateManagement.Tests;

public class MutableStateTests {

    [Fact]
    public void Set() {
        var subject = new MutableState<int>(0);

        subject.Set(42);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void Update() {
        var subject = new MutableState<int>(41);

        subject.Update(value => value + 1);

        Assert.Equal(42, subject.Value);
    }

    [Fact]
    public void ConvertFromT() {
        MutableState<int> subject = 42;

        Assert.Equal(42, subject.Value);
    }
}
