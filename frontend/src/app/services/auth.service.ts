import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User, LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromLocalStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  /**
   * Get current user from local storage
   */
  private getUserFromLocalStorage(): User | null {
    const userJson = localStorage.getItem('currentUser');
    return userJson ? JSON.parse(userJson) : null;
  }

  /**
   * User login
   */
  login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.authServiceUrl}/login`, loginRequest)
      .pipe(
        tap(response => {
          if (response.success && response.token) {
            // Construct user object from response
            const user: User = {
              id: 0,
              email: (response as any).email || '',
              role: (response as any).role || 'User',
              authProvider: 'Local',
              createdAt: new Date()
            };
            
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        })
      );
  }

  /**
   * User registration
   */
  register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.authServiceUrl}/register`, registerRequest)
      .pipe(
        tap(response => {
          if (response.success && response.token) {
            // Construct user object from response
            const user: User = {
              id: 0,
              email: (response as any).email || '',
              role: (response as any).role || 'User',
              authProvider: 'Local',
              createdAt: new Date()
            };
            
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
          }
        })
      );
  }

  /**
   * Get current user
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return !!localStorage.getItem('authToken');
  }

  /**
   * Get auth token
   */
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  /**
   * Google login with ID token
   */
  googleLogin(idToken: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${environment.authServiceUrl}/google-login`,
      { token: idToken }
    ).pipe(
      tap(response => {
        if (response.success && response.token) {
          // Construct user object from response
          const user: User = {
            id: 0,
            email: (response as any).email || '',
            role: (response as any).role || 'User',
            authProvider: 'Google',
            createdAt: new Date()
          };
          
          localStorage.setItem('authToken', response.token);
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
      })
    );
  }

  /**
   * Logout user
   */
  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
