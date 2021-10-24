var GlobalEventManager = {};

GlobalEventManager.handlers = {};

GlobalEventManager.register = function (dotNetObjectReference) {
    let keyDownHandler = function (e) {
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
        });
    }

    this.handlers[dotNetObjectReference] = keyDownHandler;

    window.addEventListener("keydown", keyDownHandler);
}

GlobalEventManager.unregister = function (dotNetObjectReference) {
    window.removeEventListener("keydown", this.handlers[dotNetObjectReference]);
    delete this.handlers[dotNetObjectReference];
}
