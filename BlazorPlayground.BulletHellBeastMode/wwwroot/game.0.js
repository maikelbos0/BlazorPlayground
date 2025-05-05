
const game = {};

function initialize(canvas, dotNetObjectReference, width, height) {
    canvas.width = width;
    canvas.height = height;

    game.canvas = canvas;
    game.context = canvas.getContext("2d");
    game.context.globalCompositeOperation = "destination-over";
    game.dotNetObjectReference = dotNetObjectReference;
    game.elements = {};
    game.renderer = window.requestAnimationFrame(render);
    game.cancellationRequested = false;

    window.addEventListener("keydown", keyDown);
    window.addEventListener("keyup", keyUp);
}

function keyDown(eventArgs) {
    if (!eventArgs.repeat) {
    game.dotNetObjectReference.invokeMethod("KeyDown", eventArgs.key);
}
}

function keyUp(eventArgs) {
    game.dotNetObjectReference.invokeMethod("KeyUp", eventArgs.key)
}

function render(timestamp) {
    if (game.previousTimestamp && game.previousTimestamp != timestamp) {
        game.context.clearRect(0, 0, game.canvas.width, game.canvas.height);

        for (const id in game.elements) {
            const element = game.elements[id];

            game.context.save();
            game.context.translate(element.position.x, element.position.y);

            element.sections.forEach((section) => {
                game.context.beginPath();
                game.context.fillStyle = section.fillColor;
                game.context.strokeStyle = section.strokeColor;
                game.context.lineWidth = section.strokeWidth;
                game.context.globalAlpha = section.opacity;

                section.coordinates.forEach((coordinate, index) => {
                    if (index === 0) {
                        game.context.moveTo(coordinate.x, coordinate.y);
                    }
                    else {
                        game.context.lineTo(coordinate.x, coordinate.y);
                    }
                });;

                game.context.fill();
                game.context.stroke();
            })

            game.context.restore();
        }

        game.dotNetObjectReference.invokeMethod("ProcessElapsedTime", timestamp - game.previousTimestamp)
    }

    if (!game.cancellationRequested) {
        game.previousTimestamp = timestamp;
        game.renderer = window.requestAnimationFrame(render);
    }
}

function addGameElement(element) {
    game.elements[element.id] = element;
}

function removeGameElement(id) {
    delete game.elements[id];
}

function terminate() {
    game.cancellationRequested = true;
    window.cancelAnimationFrame(game.renderer);

    window.removeEventListener("keydown", keyDown);
    window.removeEventListener("keyup", keyUp);
}

export { addGameElement, initialize, removeGameElement, terminate };
