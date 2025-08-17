import {InitDeleteHandler} from "../../../../shared/actions/initDeleteHandler.js";
import {deleteCardAjax} from "../../../cards/api/cardApi.js";

export function initDeleteCardHandler(onConfirm) {
  InitDeleteHandler({
    entityName: "card",
    nonTitleDescription: "цю картку",
    deleteAjax: deleteCardAjax,
    onConfirm
  });
}