import {handleWorkspaceTabs} from "./events/workspaceTabsHandler.js"
import {handleSubscribeToggle} from "../shared/events/subscribeHandler.js";
import {confirmDeleteBtn, currentWorkspace, deleteBtn, deleteModal, deleteModalText, subscribeBtn} from "./dom.js";
import {handleDeleteWorkspaceButton, initDeleteWorkspaceHandlers} from "../shared/events/deleteWorkspaceHandler.js";
import {handleParticipantSearch} from "./events/participantsSearchHandler.js";
import {initDeleteWorkspaceParticipantHandlers} from "../shared/events/deleteWorkspaceParticipantHandler.js";
import {handleUpdateWorkspaceSubmit} from "./form/updateWorkspace.js";


document.addEventListener("DOMContentLoaded", () => {
  handleWorkspaceTabs();
  handleSubscribeToggle(subscribeBtn, currentWorkspace);
  initDeleteWorkspaceHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  handleDeleteWorkspaceButton(deleteBtn, currentWorkspace, () => {
    window.location.href = "/Home/Dashboard";
  });
  handleParticipantSearch(currentWorkspace.id);
  initDeleteWorkspaceParticipantHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  handleUpdateWorkspaceSubmit();
});