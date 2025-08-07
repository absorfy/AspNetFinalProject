import { createBoardAjax } from "../../../api/boards.js";
import {
  createBoardForm,
  createBoardModal,
  boardListContainer
} from "../dom.js";

import { showBoard } from "../../shared/ui/showBoard.js";
import { getSelectedWorkspaceId } from "../load/loadBoards.js";

export function handleCreateBoardSubmit() {
  createBoardForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData(createBoardForm);
    const title = formData.get("title");
    const description = formData.get("description");
    const workspaceId = getSelectedWorkspaceId();

    if (!workspaceId) {
      alert("Не обрано робочу область.");
      return;
    }

    try {
      const newBoard = await createBoardAjax({
        workspaceId,
        title,
        description
      });

      showBoard(newBoard, boardListContainer);
      createBoardModal.hide();
      createBoardForm.reset();
    } catch (error) {
      console.error(error);
      alert("Не вдалося створити дошку.");
    }
  });
}
