﻿using System.Globalization;

namespace BlazorPlayground.Graphics {
    public class ShapeAttribute {
        public ShapeAttribute(string name, double value) : this(name, value.ToString(CultureInfo.InvariantCulture)) { }

        public ShapeAttribute(string name, string value) {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
