
const game = {};

function initialize(canvas, dotNetObjectReference, width, height) {
    game.canvas = canvas;
    game.canvas.width = width;
    game.canvas.height = height;
    game.dotNetObjectReference = dotNetObjectReference;
    game.elements = {};
}

function requestRender() {
    window.requestAnimationFrame(render);
}

function render(timestamp) {
    console.log("Rendering at: " + timestamp);

    if (game.previousTimestamp) {
        game.dotNetObjectReference.invokeMethodAsync("ProcessElapsedTime", timestamp - game.previousTimestamp)
    }

    game.previousTimestamp = timestamp;
}

function addGameElement(id, element) {
    game.elements[id] = element;
}

function removeGameElement(id) {
    delete game.elements[id];
}

export { initialize, requestRender, addGameElement, removeGameElement };
