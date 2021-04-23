async function dismissToast() {
  const toast = document.querySelector('#my-toast')
  toast.remove()
  await fetch('https://localhost:5001/Admin/RemoveAllToast')
}
