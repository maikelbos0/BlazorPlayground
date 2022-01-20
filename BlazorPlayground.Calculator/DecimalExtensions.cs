namespace BlazorPlayground.Calculator {
    public static class DecimalExtensions {
        public static decimal RemovePrecision(this decimal value) => value / 1.000000000000000000000000000000000M;
    }
}
