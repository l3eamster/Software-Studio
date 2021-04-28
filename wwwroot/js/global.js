async function dismissToast() {
  const toast = document.querySelector('#my-toast')
  toast.remove()
  await fetch('https://localhost:5001/Admin/RemoveAllToast')
}

var winX = null
var winY = null

window.addEventListener('scroll', () => {
  if (winX !== null && winY !== null) {
    window.scrollTo(winX, winY)
  }
})

function disableWindowScroll() {
  winX = window.scrollX
  winY = window.scrollY
}

function enableWindowScroll() {
  winX = null
  winY = null
}

function openLogoutPopup() {
  const popup = document.querySelector('#popupLogout')
  popup.style.opacity = 1
  popup.style.pointerEvents = 'all'
  disableWindowScroll()
}

function closeLogoutPopup() {
  const popup = document.querySelector('#popupLogout')
  popup.style.opacity = 0
  popup.style.pointerEvents = 'none'
  enableWindowScroll()
}
