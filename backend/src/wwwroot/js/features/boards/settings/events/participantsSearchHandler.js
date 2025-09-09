import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {searchParticipantsInput} from "../dom.js";
import {debounceInput} from "../../../../shared/utils/debounceInputs.js";
import {searchBoardParticipantsAjax} from "../../api/boardApi.js";
import {getNewBoardParticipantDiv} from "../ui/getBoardParticipantDiv.js";


let _inited = false;
let _searchCtrl = null;
let _triggerSearch = null;

export function initBoardParticipantsSearchHandler(workspaceId) {
  if (_inited) return;
  _inited = true;

  const resultsContainer = document.querySelector("[data-participants-search-results]");
  const view = resultsContainer ? createContainerState(resultsContainer, {skeleton: listSkeleton}) : null;

  const runSearch = async (q) => {
    if (!resultsContainer || !view) return;
    if (!q || q.trim().length < 3) {
      resultsContainer.innerHTML = "";
      return;
    }

    // cancel попереднього
    if (_searchCtrl) _searchCtrl.abort();
    _searchCtrl = new AbortController();

    view.setState(ContainerState.LOADING);
    try {
      const items = await searchBoardParticipantsAjax(workspaceId, q.trim(), _searchCtrl.signal);
      if (!_searchCtrl || _searchCtrl.signal.aborted) return;

      if (!items || items.length === 0) {
        view.setState(ContainerState.EMPTY, { message: "Нічого не знайдено" });
        return;
      }

      view.setState(ContainerState.CONTENT, {
        builder: (root) => {
          root.innerHTML = "";
          items.forEach(u => {
            const newParticipant = getNewBoardParticipantDiv(u);
            root.appendChild(newParticipant);
          });
        }
      });
    } catch (e) {
      if (_searchCtrl && _searchCtrl.signal.aborted) return;
      console.error(e);
      view.setState(ContainerState.ERROR, { message: "Помилка пошуку" });
    }
  };

  const debounced = debounceInput(runSearch, 300);

  delegate("input", {
    "search-participant-input": (inputEl) => {
      debounced(inputEl.value);
    }
  });

  _triggerSearch = () => {
    if (searchParticipantsInput) {
      runSearch(searchParticipantsInput.value);
    }
  };
}

export function triggerParticipantsSearch() {
  if (typeof _triggerSearch === "function") {
    _triggerSearch();
  }
}