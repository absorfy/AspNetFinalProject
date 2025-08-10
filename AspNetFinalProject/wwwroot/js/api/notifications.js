

export async function fetchNotifications(onlyUnread = true, take = 3) {
  const res = await fetch(`/api/notifications/?onlyUnread=${onlyUnread}?take=${take}`);
  if(!res.ok) throw new Error("Failed to fetch notifications");
  return await res.json();
}