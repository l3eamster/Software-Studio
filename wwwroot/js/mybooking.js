async function cancel(id) {
  const res = await fetch(`https://localhost:5001/Booking/Cancel/${id}`, {
    method: 'POST',
  })
  const data = await res.text()
  if (data !== 'OK') {
    alert('ERROR')
    return
  }
  alert('OK')
  location.reload()
}

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
