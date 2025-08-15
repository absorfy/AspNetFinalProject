import {initWorkspaceSettingsEvents} from "./events/eventsHandler.js";
import {createBoardForm, currentWorkspace} from "./dom.js";
import {fetchWorkspaceParticipantRoles} from "../api/workspaceApi.js";

let roles;

document.addEventListener("DOMContentLoaded", async () => {
  roles = await fetchWorkspaceParticipantRoles();
  createBoardForm.setAttribute("data-workspace-id", currentWorkspace.id);
  initWorkspaceSettingsEvents(currentWorkspace.id);
});

export function getWorkspaceRoles() {
  return roles
}