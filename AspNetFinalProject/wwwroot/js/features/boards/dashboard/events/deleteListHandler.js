import {InitDeleteHandler} from "../../../../shared/actions/initDeleteHandler.js";
import {deleteListAjax} from "../../../boardLists/api/boardListApi.js";


export function initDeleteListHandler(onConfirm) {
  InitDeleteHandler({
    entityName: "list",
    nonTitleDescription: "цей список",
    deleteAjax: deleteListAjax,
    onConfirm
  });
}