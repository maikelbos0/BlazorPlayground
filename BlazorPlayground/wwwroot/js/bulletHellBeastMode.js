
const game = {};

function initializeGame(width, height, frameRate) {
    game.canvas = document.getElementById('game');
    game.canvas.width = width;
    game.canvas.height = height;
    game.frameRate = frameRate;
}

export { initializeGame };
