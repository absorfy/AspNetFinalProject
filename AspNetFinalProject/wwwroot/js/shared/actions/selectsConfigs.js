

export function getRoleSelectConfig({targetAction, targetId, currentRole, roles, onChange, disabled = false}) {
  return {
    type: "select",
    label: "Роль:",
    className: "form-select form-select-sm w-auto",
    attrs: {
      "data-action": targetAction,
      "data-id": targetId,
      ...(disabled ? { disabled: "disabled" } : {}),
    },
    options: roles.map(r => ({
      value: r.value,
      text: r.text,
      selected: r.value === currentRole,
      ...(r.hidden ? { hidden: "hidden" } : {}),
    })),
    onChange,
  }
}