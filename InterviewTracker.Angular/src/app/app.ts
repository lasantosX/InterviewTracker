import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  get showLayout(): boolean {
    return this.router.url !== '/login';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
