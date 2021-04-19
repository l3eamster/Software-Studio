async function cancel(id) {
  const res = await fetch(`https://localhost:5001/Booking/Cancel/${id}`, {
    method: "POST"
  });
  const data = await res.text();
  if (data !== "OK") {
    alert("ERROR");
    return
  }
    alert("OK");
    location.reload();
}
