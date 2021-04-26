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
