using System.Collections.Generic;

namespace BlazorPlayground.Calculator {
    internal interface ISymbol {
        bool TryAppendTo(IList<ISymbol> symbols);
    }
}
