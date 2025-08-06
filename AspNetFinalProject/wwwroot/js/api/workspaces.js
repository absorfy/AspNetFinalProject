export async function fetchWorkspacesAjax() {
  const response = await fetch("/api/workspaces");
  if (!response.ok) throw new Error("Failed to fetch workspaces.");
  return await response.json();
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