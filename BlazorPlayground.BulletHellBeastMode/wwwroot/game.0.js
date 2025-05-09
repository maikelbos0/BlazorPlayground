
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

    window.addEventListener("keydown", addDirection);
    window.addEventListener("keyup", removeDirection);
    canvas.addEventListener("mousedown", setTargetPosition);
    canvas.addEventListener("mousemove", setTargetPosition);
    canvas.addEventListener("mouseup", resetTargetPosition);
    canvas.addEventListener("mouseleave", resetTargetPosition);
}

function addDirection(eventArgs) {
    if (!eventArgs.repeat) {
        game.dotNetObjectReference.invokeMethod("AddDirection", eventArgs.key);
    }
}

function removeDirection(eventArgs) {
    game.dotNetObjectReference.invokeMethod("RemoveDirection", eventArgs.key)
}

function setTargetPosition(eventArgs) {
    if (eventArgs.buttons & 1 === 1) {
        game.dotNetObjectReference.invokeMethod("SetTargetPosition", eventArgs.offsetX, eventArgs.offsetY);
    }
}

function resetTargetPosition() {
    game.dotNetObjectReference.invokeMethod("ResetTargetPosition");
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

        game.dotNetObjectReference.invokeMethodAsync("ProcessElapsedTime", (timestamp - game.previousTimestamp) / 1000)
    }

    if (!game.cancellationRequested) {
        game.previousTimestamp = timestamp;
        game.renderer = window.requestAnimationFrame(render);
    }
}

function setGameElement(element) {
    game.elements[element.id] = element;
}

function setGameElements(elements) {
    elements.forEach((element) => game.elements[element.id] = element);
}

function removeGameElement(id) {
    delete game.elements[id];
}

function terminate() {
    game.cancellationRequested = true;
    window.cancelAnimationFrame(game.renderer);

    window.removeEventListener("keydown", addDirection);
    window.removeEventListener("keyup", removeDirection);
}

export { initialize, setGameElement, setGameElements, removeGameElement, terminate };
