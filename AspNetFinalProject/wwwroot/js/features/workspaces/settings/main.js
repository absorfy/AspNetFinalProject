import {initWorkspaceSettingsEvents} from "./events/eventsHandler.js";
import {currentWorkspace} from "./dom.js";


document.addEventListener("DOMContentLoaded", () => {
  initWorkspaceSettingsEvents(currentWorkspace.id);
});