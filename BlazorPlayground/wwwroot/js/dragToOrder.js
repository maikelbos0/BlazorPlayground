
function getElementHeights(container) {
    const heights = [];

    for (const element of container.children) {
        heights.push(element.offsetHeight);
    }

    return heights;
}

function getPageY(container) {
    let pageY = 0;

    while (container.parentElement != document.body) {
        pageY += container.offsetTop;
        container = container.parentElement;
    }

    return pageY;
}

export { getElementHeights, getPageY }
