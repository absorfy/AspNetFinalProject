import {apiClient} from "../../../shared/api/apiClient.js"


// GET /api/workspaces/{workspaceId}/boards
export async function fetchBoardsAjax(workspaceId, query, signal) {
  return await apiClient.get(`/boards/workspace/${workspaceId}`, null, { query, signal });
}

// POST /api/boards
export async function createBoardAjax(payload, signal) {
  // payload: { title, description, workspaceId, ... }
  return await apiClient.post("/boards", payload, { signal });
}

// DELETE /api/boards/{id}
export async function deleteBoardAjax(id, signal) {
  await apiClient.delete(`/boards/${id}`, null, { signal }); // void
}

// POST /api/boards/{id}/subscribe
export async function subscribeToBoardAjax(id, signal) {
  return await apiClient.post(`/boards/${id}/subscribe`, null, { signal });
}

// DELETE /api/boards/{id}/subscribe
export async function unsubscribeFromBoardAjax(id, signal) {
  await apiClient.delete(`/boards/${id}/subscribe`, null,{ signal }); // void
}

// GET /api/boards/{id}/participants?page=1&pageSize=20
export async function fetchBoardParticipantsAjax(id, query, signal) {
  return await apiClient.get(`/boards/${id}/participants`, null, { query, signal });
}

// POST /api/boards/{id}/participants  body: { userProfileId, role }
export async function addBoardParticipantAjax(id, payload, signal) {
  return await apiClient.post(`/boards/${id}/participants`, payload, { signal });
}

// DELETE /api/boards/{id}/participants/{userProfileId}
export async function removeBoardParticipantAjax(id, userProfileId, signal) {
  await apiClient.delete(`/boards/${id}/participants`, { userProfileId }, { signal }); // void
}

export async function searchBoardParticipantsAjax(id, q, signal) {
  return await apiClient.get(`/boards/${id}/participants/search`,  null,{ query: { q }, signal });
}

// PUT /api/boards/{id}
export async function updateBoardAjax(id, payload, signal) {
  return await apiClient.put(`/boards/${id}`, payload, { signal });
}

// GET /api/boards/roles
export async function fetchBoardParticipantRoles() {
  return apiClient.get("/boards/roles");
}

export async function changeBoardParticipantRole(id, userId, newRole, signal) {
  return await apiClient.post(`/boards/${id}/participants/${userId}/role`, { role: newRole }, signal);
}

