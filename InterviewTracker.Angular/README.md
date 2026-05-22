# InterviewTracker Angular

Angular frontend for the InterviewTracker full-stack application.

## Features

- JWT login
- Protected routes with auth guard
- HTTP interceptor for Bearer tokens
- Dashboard summary cards
- Companies CRUD
- Recruiters CRUD
- Interviews CRUD
- Interview status filtering
- Pagination
- Status badge colors
- Responsive admin layout with sidebar navigation
- Unit tests with Vitest

## Tech Stack

- Angular
- TypeScript
- SCSS
- Angular Router
- Angular HttpClient
- Vitest

## Backend API

This frontend consumes the InterviewTracker ASP.NET Core API.

Default API URL:

```ts
https://localhost:7185/api
Run Locally

Install dependencies:

npm install

Start Angular:

ng serve

Open:

http://localhost:4200
Login

Demo credentials:

Username: admin
Password: password
Run Tests
ng test
Build
ng build
Main Pages
/login
/dashboard
/interviews
/companies
/recruiters
Project Goal

This frontend demonstrates an enterprise-style Angular application connected to a secured ASP.NET Core backend using JWT authentication, protected routes, reusable services, CRUD screens, pagination, filtering, and unit testing.
