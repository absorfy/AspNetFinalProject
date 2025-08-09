import {handleWorkspaceTabs} from "./events/workspaceTabsHandler.js"
import {handleSubscribeToggle} from "../shared/events/subscribeHandler.js";
import {confirmDeleteBtn, currentWorkspace, deleteBtn, deleteModal, deleteModalText, subscribeBtn} from "./dom.js";
import {handleDeleteButton, initDeleteHandlers} from "../shared/events/deleteHandler.js";


document.addEventListener("DOMContentLoaded", () => {
  handleWorkspaceTabs();
  handleSubscribeToggle(subscribeBtn, currentWorkspace);
  initDeleteHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  handleDeleteButton(deleteBtn, currentWorkspace, () => {
    window.location.href = "/Home/Dashboard";
  });
});