import {apiClient} from "../../../shared/api/apiClient.js"


// GET /api/workspaces/{workspaceId}/boards
export async function fetchBoardsAjax(workspaceId, signal) {
  return await apiClient.get(`/boards/workspace/${workspaceId}`, { signal });
}

// POST /api/boards
export async function createBoardAjax(payload, signal) {
  // payload: { title, description, workspaceId, ... }
  return await apiClient.post("/boards", payload, { signal });
}

// DELETE /api/boards/{id}
export async function deleteBoardAjax(id, signal) {
  await apiClient.delete(`/boards/${id}`, { signal }); // void
}
