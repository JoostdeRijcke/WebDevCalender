# WebDev Calendar

Calendar applicatie voor INFWAD2526

## Quick Start

```bash
npm start
```

Start backend op `http://localhost:5001` en frontend op `http://localhost:3000`

## Installatie

```bash
npm run install:all
npm start
```

## Commando's

**Starten:**
- `npm start` - Start beide servers
- `npm run start:backend` - Alleen backend
- `npm run start:frontend` - Alleen frontend

**Bouwen:**
- `npm run build:all` - Build beide projecten
- `npm run build:backend` - Build backend
- `npm run build:frontend` - Build frontend

## Tech Stack

**Backend:**
- ASP.NET Core 8.0
- Entity Framework Core 8.0.11
- SQLite
- Repository Pattern

**Frontend:**
- React 19.0
- TypeScript 4.4.2
- React Router DOM 7.1.1

## Project Structuur

```
Project/
├── Backend/
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Services/
│   ├── Controllers/
│   ├── Models/
│   └── Seeders/
└── frontend/
    └── src/
        ├── api/
        ├── pages/
        ├── components/
        └── styling/
```

## API Endpoints

Base URL: `http://localhost:5001/api`

**Authentication:**
- `POST /Login`
- `POST /Register`
- `GET /Logout`
- `GET /IsUserLoggedIn`
- `GET /IsAdminLoggedIn`

**Events:**
- `GET /Events`
- `GET /Events/{id}`
- `POST /Events`
- `PUT /Events/{id}`
- `DELETE /Events/{id}`
- `GET /Events/search`

**Attendance:**
- `POST /EventAttendance/attend`
- `DELETE /EventAttendance/remove`
- `GET /EventAttendance/attendees/{eventId}`
- `GET /EventAttendance/user/{userId}/attended-events`

**Reviews:**
- `GET /Events/reviews`
- `POST /Events/review`

## Development

**Backend development:**
```bash
cd Project/Backend
dotnet watch run
```

**Frontend development:**
```bash
cd Project/frontend
npm start
```

## Database

- Type: SQLite
- Locatie: `Project/Backend/calendify.db`
- Migrations: Entity Framework Core
- Seeding: Automatisch bij startup

## Architectuur

Repository Pattern implementatie:

```
Controller → Service → Repository → Database
```

Separation of concerns tussen business logic (Services) en data access (Repositories).
