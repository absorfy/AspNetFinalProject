export const ContainerState = {
  LOADING: "loading",
  EMPTY: "empty",
  ERROR: "error",
  CONTENT: "content",
};

/**
 * Маленький стейт-машин для контейнера зі списком/блоком.
 * Підтримує: loading, empty, error, content.
 */
export function createContainerState(container, { skeleton = null } = {}) {
  if (!container) throw new Error("container is required");

  let current = null;

  const renderLoading = () => {
    container.innerHTML = "";
    if (skeleton) {
      container.appendChild(makeNode(skeleton));
    } else {
      container.appendChild(makeNode(`<div class="text-muted py-2">Завантаження…</div>`));
    }
  };

  const renderEmpty = (message = "Порожньо") => {
    container.innerHTML = `<div class="text-muted py-2">${escapeHtml(message)}</div>`;
  };

  const renderError = (message = "Не вдалося завантажити") => {
    container.innerHTML = `<div class="text-danger py-2">${escapeHtml(message)}</div>`;
  };

  const renderContent = (builder) => {
    // builder(container) — ти всередині наповнюєш DOM як хочеш
    container.innerHTML = "";
    builder(container);
  };

  function setState(state, payload) {
    if (current === state) return;
    current = state;

    switch (state) {
      case ContainerState.LOADING:
        renderLoading();
        break;
      case ContainerState.EMPTY:
        renderEmpty(payload?.message);
        break;
      case ContainerState.ERROR:
        renderError(payload?.message);
        break;
      case ContainerState.CONTENT:
        setTimeout(() => {
          renderContent(payload?.builder ?? (() => {}));
        }, 500)
        
        break;
      default:
        renderError("Невідомий стан контейнера");
    }
  }

  return { setState, get state() { return current; } };
}

function makeNode(html) {
  const tpl = document.createElement("template");
  tpl.innerHTML = html.trim();
  return tpl.content.firstChild;
}

function escapeHtml(s) {
  return String(s).replace(/[&<>"']/g, (ch) =>
    ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;" }[ch])
  );
}
