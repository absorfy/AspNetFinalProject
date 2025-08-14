import {ApiError} from "../api/apiClient.js";

export function initConfirmDelete({ modalElement, confirmBtn, modalTextElement }) {
  if (!modalElement || !confirmBtn || !modalTextElement) {
    throw new Error("Не передані всі елементи для confirmDelete");
  }

  const bsModal = bootstrap.Modal.getOrCreateInstance(modalElement);
  let currentAction = null; // { title, onConfirm }

  // натискання "Підтвердити"
  confirmBtn.addEventListener("click", async () => {
    if (!currentAction) return;

    confirmBtn.disabled = true;
    try {
      await currentAction.onConfirm();
      bsModal.hide();
      currentAction = null;
    } catch (err) {
      if (err instanceof ApiError) {
        alert(`Помилка: ${err.message}`);
      } else {
        alert("Сталася невідома помилка.");
        console.error(err);
      }
    } finally {
      confirmBtn.disabled = false;
    }
  });

  return function confirmDelete({ title, onConfirm }) {
    currentAction = { title, onConfirm };
    modalTextElement.textContent = `Ви дійсно хочете видалити ${title}?`;
    bsModal.show();
  };
}