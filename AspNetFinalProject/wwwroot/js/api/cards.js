export async function fetchCardsByList(listId) {
  const res = await fetch(`/api/cards/list/${listId}`);
  if (!res.ok) throw new Error("Failed to fetch cards");
  return await res.json();
}

export async function createCardAjax(data) {
  const res = await fetch("/api/cards", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });
  if (!res.ok) throw new Error("Failed to create card");
  return await res.json();
}
