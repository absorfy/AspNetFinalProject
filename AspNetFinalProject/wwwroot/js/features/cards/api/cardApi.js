import { apiClient } from "../../../shared/api/apiClient.js";

// GET /api/cards/list/{listId}
export async function fetchCardsByList(listId, signal) {
  return await apiClient.get(`/cards/list/${listId}`, null, { signal });
}

// POST /api/cards
export async function createCardAjax(payload, signal) {
  return await apiClient.post("/cards", payload, { signal });
}

export async function changeListForCard(cardId, newListId, signal) {
  return await apiClient.post(`/cards/${cardId}/change-list/${newListId}`, null, { signal });
}

export async function deleteCardAjax(cardId, signal) {
  await apiClient.delete(`/cards/${cardId}`, null, { signal });
}