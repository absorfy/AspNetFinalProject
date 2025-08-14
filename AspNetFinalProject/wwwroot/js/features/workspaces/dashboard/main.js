import { loadWorkspaces } from "./load/loadWorkspaces.js";
import {initWorkspaceDashboardEvents} from "./events/eventsHandler.js";
import {workspaceListContainer} from "./dom.js";


document.addEventListener("DOMContentLoaded", async () => {
  initWorkspaceDashboardEvents();
  await loadWorkspaces(workspaceListContainer);
});