function open_popup(id) {
    const popup = document.querySelector("#popup" + id)
    popup.style.opacity = 1;
    popup.style.pointerEvents = "all";
}

function close_popup(id) {
    const popup = document.querySelector("#popup" + id)
    popup.style.opacity = 0;
    popup.style.pointerEvents = "none";
}