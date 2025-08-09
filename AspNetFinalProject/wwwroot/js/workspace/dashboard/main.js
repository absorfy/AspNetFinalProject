import { loadWorkspaces } from "./load/loadWorkspaces.js";
import { handleCreateWorkspaceSubmit } from "./form/createWorkspace.js";
import { handleCreateBoardSubmit } from "./form/createBoard.js";
import {confirmDeleteBtn, deleteModal, deleteModalText} from "./dom.js";
import {initDeleteHandlers} from "../shared/events/deleteHandler.js";


document.addEventListener("DOMContentLoaded", () => {
  loadWorkspaces();
  initDeleteHandlers({confirmDeleteBtn, deleteModal, deleteModalText});
  handleCreateWorkspaceSubmit();
  handleCreateBoardSubmit();
});