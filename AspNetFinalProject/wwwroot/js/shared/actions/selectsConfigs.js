

export function getRoleSelectConfig({targetAction, targetId, currentRole, roles}) {
  return {
    type: "select",
    label: "Роль:",
    className: "form-select form-select-sm w-auto",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
    },
    options: roles.map(r => ({
      value: r.value,
      text: r.text,
      selected: r.value === currentRole,
    })),
  }
}