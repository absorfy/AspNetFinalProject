import {deleteBoardAjax} from "../../../boards/api/boardApi.js";
import {InitDeleteHandler} from "../../../../shared/actions/initDeleteHandler.js";

export function initDeleteBoardHandler(onConfirm) {
  InitDeleteHandler({
    entityName: "board",
    nonTitleDescription: "цю дошку",
    deleteAjax: deleteBoardAjax,
    onConfirm
  });
}