export async function fetchWorkspacesAjax() {
  const response = await fetch("/api/workspaces");
  if (!response.ok) throw new Error("Failed to fetch workspaces.");
  return await response.json();
}

export async function updateWorkspaceAjax(workspaceId, data) {
  const response = await fetch(`/api/workspaces/${workspaceId}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to update workspace. Server response: ${errorText}`);
  }
}

export async function createWorkspaceAjax(data) {
  const response = await fetch("/api/workspaces/", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  })
  if(!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to create workspace. Server response: ${errorText}`);
  }
  return await response.json();
}

export async function deleteWorkspaceAjax(workspaceId) {
  const response = await fetch(`/api/workspaces/${workspaceId}`, {
    method: "DELETE"
  });
  if(!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to delete workspace. Server response: ${errorText}`);
  }
}

export async function subscribeToWorkspaceAjax(workspaceId) {
  const response = await fetch(`/api/workspaces/${workspaceId}/subscribe`, {
    method: "POST"
  });
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to subscribe to workspace. Server response: ${errorText}`);
  }
}

export async function unsubscribeFromWorkspaceAjax(workspaceId) {
  const response = await fetch(`/api/workspaces/${workspaceId}/subscribe`, {
    method: "DELETE"
  });
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Failed to unsubscribe from workspace. Server response: ${errorText}`);
  }
}

export async function fetchWorkspaceParticipantsAjax(workspaceId) {
  const response = await fetch(`/api/workspaces/${workspaceId}/participants`);
  if (!response.ok) throw new Error("Failed to fetch workspace participants.");
  return await response.json();
}