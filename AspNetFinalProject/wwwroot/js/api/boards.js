export async function fetchBoardsAjax(workspaceId) {
  const response = await fetch(`/api/boards/workspace/${workspaceId}`);
  if(!response.ok) throw new Error("Failed to fetch boards.");
  return await response.json();
}

export async function createBoardAjax(data) {
  const response = await fetch("/api/boards", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  })
  if(!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create board. Server response: ${errorText}`);
  }
  return await response.json();
}

export async function deleteBoardAjax(boardId) {
  const response = await fetch(`/api/boards/${boardId}`, {
    method: "DELETE",
  });
  if(!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete board. Server response: ${errorText}`);
  }
}