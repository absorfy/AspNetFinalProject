
import {apiClient} from "../../../shared/api/apiClient.js";

export async function fetchWorkspacesAjax({ page = 1, pageSize = 20 } = {}, signal) {
  return await apiClient.get("/workspaces", null, { query: { page, pageSize }, signal });
}

// POST /api/workspaces
export async function createWorkspaceAjax(payload, signal) {
  // payload: { title, description, visibility, ... }
  return await apiClient.post("/workspaces", payload, { signal });
}

// DELETE /api/workspaces/{id}
export async function deleteWorkspaceAjax(id, signal) {
  await apiClient.delete(`/workspaces/${id}`, null, { signal }); // void
}

// POST /api/workspaces/{id}/subscribe
export async function subscribeToWorkspaceAjax(id, signal) {
  return await apiClient.post(`/workspaces/${id}/subscribe`, null, { signal });
}

// DELETE /api/workspaces/{id}/subscribe
export async function unsubscribeFromWorkspaceAjax(id, signal) {
  await apiClient.delete(`/workspaces/${id}/subscribe`, null,{ signal }); // void
}

// GET /api/workspaces/{id}/participants?page=1&pageSize=20
export async function fetchWorkspaceParticipantsAjax(id, { page = 1, pageSize = 20 } = {}, signal) {
  return await apiClient.get(`/workspaces/${id}/participants`, null, { query: { page, pageSize }, signal });
}

// POST /api/workspaces/{id}/participants  body: { userProfileId, role }
export async function addWorkspaceParticipantAjax(id, payload, signal) {
  return await apiClient.post(`/workspaces/${id}/participants`, payload, { signal });
}

// DELETE /api/workspaces/{id}/participants/{userProfileId}
export async function removeWorkspaceParticipantAjax(id, userProfileId, signal) {
  await apiClient.delete(`/workspaces/${id}/participants`, { userProfileId }, { signal }); // void
}

// GET /api/workspaces/{id}/participants/search?q=...
// Міндовжина q -> на рівні UI (debounce + перевірка), НЕ тут
export async function searchWorkspaceParticipantsAjax(id, q, signal) {
  return await apiClient.get(`/workspaces/${id}/participants/search`,  null,{ query: { q }, signal });
}

// PUT /api/workspaces/{id}
export async function updateWorkspaceAjax(id, payload, signal) {
  return await apiClient.put(`/workspaces/${id}`, payload, { signal });
}

// GET /workspaces/roles
export async function fetchWorkspaceParticipantRoles() {
  return await apiClient.get("/workspaces/roles");
}

export async function changeWorkspaceParticipantRole(id, userId, newRole, signal) {
  return await apiClient.post(`/workspaces/${id}/participants/${userId}/role`, { role: newRole }, signal);
}