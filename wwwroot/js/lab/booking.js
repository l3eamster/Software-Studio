/**
 * Functions
 */
function toggleCheckbox(event) {
  const timeline = event.target.parentElement.parentElement
  timeline.classList.toggle('timeline-selected')
}

/**
 * Make reset button work!
 */
const form = document.querySelector('#form')
const resetBtn = document.querySelector('#reset-btn')

resetBtn.onclick = () => {
  form.reset()
  const checkboxs = form.querySelectorAll('input')
  checkboxs.forEach((c) => {
    if (!c.disabled)
      c.parentElement.parentElement.classList.remove('timeline-selected')
  })
}

/**
 * Form handler
 */
async function submitHandler(event, labId, username, userId) {
  event.preventDefault()

  if (!username) {
    alert('ไปล็อคอินก๊อนน')
    window.location = 'https://localhost:5001/'
    return
  }

  const result = []
  const timelines = form.querySelectorAll('.timeline-zone')

  timelines.forEach((tl) => {
    const Booking = []
    const times = tl.querySelectorAll('.timeline')

    times.forEach((t) => {
      const checkbox = t.querySelector('input')
      Booking.push(checkbox.checked)
    })

    if (Booking.some((b) => b === true))
      result.push({ Date: tl.dataset.date, Booking })
  })

  // Post form
  const res = await fetch('https://localhost:5001/Lab/Booking', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ UserId: userId, LabId: labId, BookingList: result }),
  })
  const data = await res.text()
  if (data === 'ERROR') alert('ERROR!')
  else {
    alert('OK')
    location.reload()
  }
}
