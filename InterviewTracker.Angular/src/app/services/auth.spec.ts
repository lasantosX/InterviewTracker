import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';

import { AuthService } from './auth';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();

    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should login and store access token', () => {
    service.login({ username: 'admin', password: 'password' }).subscribe(response => {
      expect(response.accessToken).toBe('fake-token');
    });

    const req = httpMock.expectOne('https://localhost:7185/api/Auth/login');

    expect(req.request.method).toBe('POST');

    req.flush({
      accessToken: 'fake-token'
    });

    expect(localStorage.getItem('accessToken')).toBe('fake-token');
  });

  it('should remove token on logout', () => {
    localStorage.setItem('accessToken', 'fake-token');

    service.logout();

    expect(localStorage.getItem('accessToken')).toBeNull();
  });

  it('should return true when authenticated', () => {
    localStorage.setItem('accessToken', 'fake-token');

    expect(service.isAuthenticated()).toBe(true);
  });

  it('should return false when not authenticated', () => {
    expect(service.isAuthenticated()).toBe(false);
  });
});
