var GlobalEventManager = {};

GlobalEventManager.register = function (dotNetObjectReference) {
    function keyDownHandler(e) {
        // TODO expand with full keydown event args
        dotNetObjectReference.invokeMethodAsync("OnKeyDown", {
            key: e.key
        })
            .catch(function (e) {
                console.log(e);
                window.removeEventListener("keydown", keyDownHandler);
            });
    }

    window.addEventListener("keydown", keyDownHandler);
}
