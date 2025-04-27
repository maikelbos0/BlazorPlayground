
const game = {};

function initialize(id, width, height, frameRate) {
    game.canvas = document.getElementById(id);
    game.canvas.width = width;
    game.canvas.height = height;
    game.frameRate = frameRate;
    game.elements = {};
}

export { initialize };
