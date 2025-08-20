import {apiClient} from "../../../shared/api/apiClient.js";

// GET /api/notifications?onlyUnread=&take=
export async function fetchNotifications(onlyUnread = true, take = 100, signal) {
  return await apiClient.get("/notifications", null, { query: { onlyUnread, take }, signal });
}

// POST /api/notifications/{id}/read
export async function markAsReadNotification(notificationId, signal) {
  await apiClient.post(`/notifications/${notificationId}/read`, null, { signal }); // void відповіді не очікуємо
}