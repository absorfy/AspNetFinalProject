import {initWorkspaceSettingsEvents} from "./events/eventsHandler.js";
import {createBoardForm, currentWorkspace} from "./dom.js";
import {fetchWorkspaceParticipantRoles} from "../api/workspaceApi.js";

let roleValues;

document.addEventListener("DOMContentLoaded", async () => {
  roleValues = await fetchWorkspaceParticipantRoles();
  createBoardForm.setAttribute("data-workspace-id", currentWorkspace.id);
  initWorkspaceSettingsEvents(currentWorkspace.id);
});

export function getWorkspaceRoles() {
  return roleValues
}