/**
 * Простий делегатор подій.
 * @param {string} eventType - тип події (наприклад, "click")
 * @param {Object<string, Function>} actions - мапа: { "data-action": handler }
 * @param {HTMLElement} [root=document] - контейнер для прослуховування
 */
export function delegate(eventType, actions, root = document) {
  root.addEventListener(eventType, (e) => {
    const target = e.target.closest("[data-action]");
    if (!target) return;
    const action = target.dataset.action;
    if (action && actions[action]) {
      actions[action](target, e);
    }
  });
}
