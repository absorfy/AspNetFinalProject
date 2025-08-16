import {fetchBoardsAjax} from "../../../boards/api/boardApi.js";
import {getBoardDiv} from "../ui/getBoardDiv.js";
import {createPaginationController} from "../../../../shared/ui/paginationController.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {bindDebouncedInput} from "../../../../shared/utils/debounceInputs.js";

export function loadBoardsWithWorkspaceId(workspaceId, container) {
  if(!container || !workspaceId) return;

  const ctrl = createPaginationController({
    root: container,
    controlsPosition: "top",
    async fetchPage(state, signal) {
      // state: { page, pageSize, search, sortBy, descending }
      return await fetchBoardsAjax(workspaceId, {
        page: state.page,
        pageSize: state.pageSize,
        search: state.search,
        sortBy: state.sortBy,
        descending: state.descending
      }, signal);
    },
    renderItem: getBoardDiv,
    renderSkeleton: () => listSkeleton,
    inputId: "boardsSearchInput",
    controllerId: "boardPaginationController",
    emptyMessage: "Ще немає дошок",
    errorMessage: "Не вдалося завантажити дошки",
    initialState: { page: 1, pageSize: 10, sortBy: "", descending: true },
    pageSizeOptions: [5, 10, 20, 50],
    sortOptions: [
      { value: "", text: "Без сортування" },
      { value: "title", text: "За назвою" },
      { value: "date", text: "За датою" },
      { value: "author", text: "За автором" },
    ],
    searchPlaceholder: "Пошук дошок..."
  });

  bindDebouncedInput({
    element: "#boardsSearchInput", 
    delay: 300,
    minLength: 3,
    onChange: (q) => ctrl.setState({ search: q, page: 1 })
  });
  
  return ctrl;
}
