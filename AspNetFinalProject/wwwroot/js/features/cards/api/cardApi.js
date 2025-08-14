import { apiClient } from "../../../shared/api/apiClient.js";

// GET /api/cards/list/{listId}
export async function fetchCardsByList(listId, signal) {
  return await apiClient.get(`/cards/list/${listId}`, { signal });
}

// POST /api/cards
export async function createCardAjax(payload, signal) {
  // payload: { boardListId, title, description?, ... }
  return await apiClient.post("/cards", payload, { signal });
}