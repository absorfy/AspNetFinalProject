import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {createBoardListAjax} from "../../../boardLists/api/boardListApi.js";
import {showBoardList} from "../ui/showBoardList.js";
import {boardListsContainer} from "../dom.js";


export function initCreateBoardListHandler(currentBoardId) {
  delegate("submit", {
    "create-board-list-form": async (form, e) => {
      e.preventDefault();
      const data = { title: form.title.value, boardId: currentBoardId };
      try {
        const newList = await createBoardListAjax({ ...data, boardId: currentBoardId });
        form.reset();
        bootstrap.Modal.getInstance(form.closest(".modal")).hide();
        showBoardList(newList, boardListsContainer)
      } catch (err) {
        alert(`Не вдалося створити список: ${err.message}`);
      }
    },
  });
}