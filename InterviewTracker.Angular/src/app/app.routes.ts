import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login';
import { Dashboard } from './pages/dashboard/dashboard';
import { Companies } from './pages/companies/companies';
import { Recruiters } from './pages/recruiters/recruiters';
import { Interviews } from './pages/interviews/interviews';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },

  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'companies', component: Companies, canActivate: [authGuard] },
  { path: 'recruiters', component: Recruiters, canActivate: [authGuard] },
  { path: 'interviews', component: Interviews, canActivate: [authGuard] },

  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'dashboard' },
];
