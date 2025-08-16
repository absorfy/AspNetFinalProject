import {apiClient} from "../../../shared/api/apiClient.js";

// GET /api/lists/board/{boardId}
export async function fetchListsByBoard(boardId, signal) {
  return await apiClient.get(`/lists/board/${boardId}`, null, { signal });
}

// POST /api/lists
export async function createBoardListAjax(payload, signal) {
  return await apiClient.post("/lists", payload, null, { signal });
}
