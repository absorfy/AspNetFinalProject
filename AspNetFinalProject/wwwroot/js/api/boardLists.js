export async function fetchListsByBoard(boardId) {
  const res = await fetch(`/api/lists/board/${boardId}`);
  if (!res.ok) throw new Error("Failed to fetch board lists");
  return await res.json();
}

export async function createBoardListAjax(data) {
  const res = await fetch("/api/lists", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });
  if (!res.ok) throw new Error("Failed to create board list");
  return await res.json();
}