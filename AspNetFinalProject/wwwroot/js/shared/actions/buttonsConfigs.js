

export function getSettingsButtonConfig({targetAction, targetId}) {
  return {
    text: "Налаштувати",
    className: "btn btn-sm btn-outline-secondary",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId
    }
  }
}

export function getSubscribeButtonConfig({targetAction, targetId, isSubscribed}) {
  return {
    text: isSubscribed ? "Відписатися" : "Підписатися",
    className: isSubscribed
      ? "btn btn-sm btn-danger"
      : "btn btn-sm btn-success",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      "data-subscribed": isSubscribed,
    },
  }
}

export function getDeleteButtonConfig({targetAction, targetId, targetTitle}) {
  return {
    text: "Видалити",
    className: "btn btn-sm btn-outline-danger",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      "data-title": targetTitle
    }
  }
}