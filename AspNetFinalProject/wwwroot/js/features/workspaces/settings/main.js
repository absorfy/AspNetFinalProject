import {initWorkspaceSettingsEvents} from "./events/eventsHandler.js";
import {createBoardForm, currentWorkspace} from "./dom.js";


document.addEventListener("DOMContentLoaded", () => {
  createBoardForm.setAttribute("data-workspace-id", currentWorkspace.id);
  initWorkspaceSettingsEvents(currentWorkspace.id);
});