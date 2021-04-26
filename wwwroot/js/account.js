function openPopup(id) {
  const popup = document.querySelector('#popup' + id)
  popup.style.opacity = 1
  popup.style.pointerEvents = 'all'
  disableWindowScroll()
}

function closePopup(id) {
  const popup = document.querySelector('#popup' + id)
  popup.style.opacity = 0
  popup.style.pointerEvents = 'none'
  enableWindowScroll()
}
