# Kanban

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Framework-5C2D91?logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green)
![Identity](https://img.shields.io/badge/Identity-Security-orange)
![Razor Pages](https://img.shields.io/badge/Razor-Pages-blueviolet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-Frontend-purple?logo=bootstrap&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57?logo=sqlite&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-Realtime-red)
![SortableJS](https://img.shields.io/badge/SortableJS-Drag%20%26%20Drop-yellow)

> A Trello-like task management web application built with ASP.NET Core.  
> It allows users to organize their workspaces, boards, lists, and cards with real-time updates and role-based access control.

---

## 📖 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Usage](#usage)
- [Repository Structure](#repository-structure)
- [Author](#author)

---

## 📌 Overview
**Kanban** is a project management application inspired by Trello.  
It provides functionality for workspaces, boards, lists, and cards, with customizable roles, notifications, and drag-and-drop interaction. The system is designed for collaboration and personal productivity, featuring pagination, action logs, and real-time updates.

---

## ✨ Features
- **Workspaces & Boards**
  - Create and manage workspaces and boards
  - Configure visibility (private / public / workspace-only)
  - Assign members with different roles
  - Subscribe to boards and workspaces for activity updates

- **Lists & Cards**
  - Add lists with custom names
  - Create cards with description
  - Drag & Drop support for cards (`SortableJS` integration)

- **User Profiles**
  - Manage personal information and visibility
  - Activity logs (user action history)

- **Collaboration**
  - Add participants to workspaces and boards
  - Role-based permissions
  - Real-time notifications (`SignalR`)

- **Other**
  - Pagination for boards, workspaces, and participants

---

## 🛠 Technologies
- **Backend:** ASP.NET Core, Entity Framework Core, Identity  
- **Frontend:** Razor Pages, JavaScript, Bootstrap  
- **Database:** SQLite  
- **Libraries & Tools:**  
  - [SignalR](https://learn.microsoft.com/aspnet/core/signalr)  
  - [SortableJS](https://sortablejs.github.io/Sortable/)

---

## ⚙️ Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/absorfy/AspNetFinalProject.git
   cd AspNetFinalProject
   ```

2. Set up the database (SQLite by default).  
   Apply migrations:
   ```bash
   dotnet ef database update
   ```

---

## ▶️ Usage
Run the application locally:
```bash
dotnet run
```
The application will be available at: [http://localhost:5134](http://localhost:5000)

---

## 👨‍💻 Author
**Vladyslav Krykun**  
📧 vladyslav.krykun89@gmail.com  
🌐 [GitHub Profile](https://github.com/absorfy)
