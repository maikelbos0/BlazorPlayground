var GlobalEventManager = {};

GlobalEventManager.register = function (dotNetObjectReference) {
    function keyDownHandler(e) {
        console.log(e);

        dotNetObjectReference.invokeMethodAsync("OnKeyDown", {
            altKey: e.altKey,
            code: e.code,
            ctrlKey: e.ctrlKey,
            key: e.key,
            location: e.location,
            metaKey: e.metaKey,
            repeat: e.repeat,
            shiftKey: e.shiftKey,
            type: 'keydown'
        })
            .catch(function (e) {
                console.log(e);
                window.removeEventListener("keydown", keyDownHandler);
            });
    }

    window.addEventListener("keydown", keyDownHandler);
}
