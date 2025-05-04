
const game = {};

function initialize(canvas, dotNetObjectReference, width, height) {
    canvas.width = width;
    canvas.height = height;

    game.cancellationRequested = false;
    game.canvas = canvas;
    game.context = canvas.getContext("2d");
    game.context.globalCompositeOperation = "destination-over";
    game.dotNetObjectReference = dotNetObjectReference;
    game.elements = {};

    window.requestAnimationFrame(render);
}

function render(timestamp) {
    if (game.cancellationRequested) {
        return;
    }

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

        game.dotNetObjectReference.invokeMethodAsync("ProcessElapsedTime", timestamp - game.previousTimestamp)
    }


    if (!game.cancellationRequested) {
        game.previousTimestamp = timestamp;
        window.requestAnimationFrame(render);
    }
}

function addGameElement(element) {
    game.elements[element.id] = element;
}

function removeGameElement(id) {
    delete game.elements[id];
}

function requestCancellation() {
    game.cancellationRequested = true;
}

export { addGameElement, initialize, removeGameElement, requestCancellation };
