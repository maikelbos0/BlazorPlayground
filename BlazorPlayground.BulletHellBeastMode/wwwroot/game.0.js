
const game = {};

function initialize(canvas, width, height, frameRate) {
    game.canvas = canvas;
    game.canvas.width = width;
    game.canvas.height = height;
    game.frameRate = frameRate;
    game.elements = {};
}

function addGameElement(id, element) {
    game.elements[id] = element;
    console.log(game.elements);
}

function removeGameElement(id) {
    delete game.elements[id];
    console.log(game.elements);
}

export { initialize, addGameElement, removeGameElement };
