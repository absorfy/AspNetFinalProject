import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createBoardAjax} from "../../../boards/api/boardApi.js";
import {getBoardDiv} from "../ui/getBoardDiv.js";


export function initCreateBoardSubmitHandler(container) {
  delegate("submit", {
    "create-board-form": async (form, e) => {
      e.preventDefault();
      const data = Object.fromEntries(new FormData(form).entries());
      try {
        const newBoard = await createBoardAjax({...data, workspaceId: form.getAttribute("data-workspace-id")});
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal"))?.hide();
        container.appendChild(getBoardDiv(newBoard));
      } catch (err) {
        alert(`Не вдалося створити дошку: ${err.message}`);
      }
    },
  });
}