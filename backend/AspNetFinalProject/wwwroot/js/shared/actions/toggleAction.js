import { ApiError } from "../api/apiClient.js";

/**
 * Універсальний обробник тумблерів (subscribe, mark-as-read, like, і т.д.)
 * @param {Object} opts
 * @param {HTMLElement} btn - кнопка, яка тригерить дію
 * @param {Function} getState - () => boolean — поточний стан (true=увімкнено)
 * @param {Function} setState - (newState: boolean) => void — оновлення UI після зміни стану
 * @param {Function} doAjax - () => Promise — виконує запит до API
 * @param {string} [loadingText] - текст кнопки у стані "завантаження"
 */
export async function toggleAction({ btn, getState, setState, doAjax, loadingText }) {
  if (!btn) return;

  const prevState = getState();
  const prevText = btn.textContent;

  btn.disabled = true;
  if (loadingText) btn.textContent = loadingText;

  try {
    await doAjax();
    setState(!prevState);
  } catch (err) {
    if (err instanceof ApiError) {
      alert(`Помилка: ${err.message}`);
    } else {
      alert("Сталася невідома помилка.");
      console.error(err);
    }
    setState(prevState); // rollback
    btn.textContent = prevText;
  } finally {
    btn.disabled = false;
  }
}
