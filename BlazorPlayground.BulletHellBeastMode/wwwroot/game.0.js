
const game = {};

function initialize(canvas, width, height, frameRate) {
    game.canvas = canvas;
    game.canvas.width = width;
    game.canvas.height = height;
    game.frameRate = frameRate;
    game.elements = {};
}

export { initialize };
