

export function getSettingsButtonConfig({targetAction, targetId, hidden = false}) {
  return {
    text: "Налаштувати",
    className: "btn btn-sm btn-outline-secondary",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      ...(hidden ? { hidden: "hidden" } : {}),
    }
  }
}

export function getSubscribeButtonConfig({targetAction, targetId, isSubscribed, hidden = false}) {
  return {
    text: isSubscribed ? "Відписатися" : "Підписатися",
    className: isSubscribed
      ? "btn btn-sm btn-danger"
      : "btn btn-sm btn-success",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      "data-subscribed": isSubscribed,
      ...(hidden ? { hidden: "hidden" } : {}),
    },
  }
}

export function getDeleteButtonConfig({targetAction, targetId, targetTitle, hidden = false}) {
  return {
    text: "Видалити",
    className: "btn btn-sm btn-outline-danger",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      "data-title": targetTitle,
      ...(hidden ? { hidden: "hidden" } : {}),
    }
  }
}