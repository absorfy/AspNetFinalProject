import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createBoardAjax} from "../../../boards/api/boardApi.js";
import {getBoardPaginationController} from "../load/loadBoards.js";


export function initCreateBoardSubmitHandler(container) {
  delegate("submit", {
    "create-board-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      try {
        await createBoardAjax({...data, workspaceId: form.getAttribute("data-workspace-id")});
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal"))?.hide();
        getBoardPaginationController().refresh();
      } catch (err) {
        alert(`Не вдалося створити дошку: ${err.message}`);
      }
    },
  });
}