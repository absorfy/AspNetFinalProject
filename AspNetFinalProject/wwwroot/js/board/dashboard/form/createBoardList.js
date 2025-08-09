import {createBoardListAjax} from "../../../api/boardLists.js";
import {boardListsContainer, createListForm, createListModal, currentBoardId} from "../dom.js";
import {showBoardList} from "../ui/showBoardList.js";


export const handleCreateBoardListSubmit = () => {
  createListForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const formData = new FormData(createListForm);
    const title = formData.get("title");

    try {
      const newList = await createBoardListAjax({ boardId: currentBoardId, title });
      showBoardList(newList, boardListsContainer);
      createListModal.hide();
      createListForm.reset();
    } catch (err) {
      console.error(err);
      alert("Не вдалося створити список.");
    }
  });
}