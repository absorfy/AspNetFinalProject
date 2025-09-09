import {loadParticipants} from "../load/loadParticipants.js";
import {ContainerState, createContainerState} from "../../../../shared/ui/containerState.js";
import {delegate} from "../../../../shared/utils/eventDelegator.js";
import {loadBoards} from "../../shared/load/loadBoards.js";
import {listSkeleton} from "../../../../shared/ui/skeletons.js";
import {boardContainer, participantContainer} from "../dom.js";


let _inited = false;
const DEFAULT_TAB = "general";

function setActiveTabButton(tab) {
  document.querySelectorAll(`[data-action="open-tab"]`).forEach(btn => {
    btn.classList.toggle("active", btn.dataset.tab === tab);
    btn.setAttribute("aria-current", btn.dataset.tab === tab ? "page" : "false");
  });
}

function showOnlyTab(tab) {
  document.querySelectorAll(`[data-tab-content]`).forEach(el => {
    el.classList.toggle("d-none", el.dataset.tabContent !== tab);
  });
}

async function loadTab(tab, { workspaceId }) {
  // показати лише потрібний таб
  showOnlyTab(tab);
  setActiveTabButton(tab);

  // завантаження контенту конкретної вкладки
  switch (tab) {
    case "participants":
      await loadParticipants(workspaceId, participantContainer);
      break;
    case "boards":
      await loadBoards(workspaceId, boardContainer);
      break;
    case "general":
    default:
  }
}

function getTabFromHash() {
  const h = (location.hash || "").replace(/^#/, "").trim();
  return h || DEFAULT_TAB;
}

function setHash(tab) {
  if (getTabFromHash() !== tab) history.replaceState(null, "", `#${tab}`);
}

export function initWorkspaceTabsHandler(opts) {
  if (_inited) return; // ідемпотентність
  _inited = true;

  
  const { workspaceId } = opts || {};
  // делегування кліків по табам
  delegate("click", {
    "open-tab": (btn, e) => {
      e.preventDefault();
      const tab = btn.dataset.tab || DEFAULT_TAB;
      setHash(tab);
      loadTab(tab, { workspaceId });
    }
  });

  // реагуємо на зміну hash (якщо треба)
  window.addEventListener("hashchange", () => {
    loadTab(getTabFromHash(), { workspaceId });
  });

  // перший запуск — із hash або за замовчуванням
  const initial = getTabFromHash();
  setHash(initial);
  loadTab(initial, { workspaceId });
}
