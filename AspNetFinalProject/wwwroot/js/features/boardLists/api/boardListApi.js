import {apiClient} from "../../../shared/api/apiClient.js";

// GET /api/lists/board/{boardId}
export async function fetchListsByBoard(boardId, signal) {
  return await apiClient.get(`/lists/board/${boardId}`, { signal });
}

// POST /api/lists
export async function createBoardListAjax(payload, signal) {
  // payload: { boardId, title, ... }
  return await apiClient.post("/lists", payload, { signal });
}
