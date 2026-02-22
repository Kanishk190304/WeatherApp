import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpInterceptorFn } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

/**
 * Functional HTTP interceptor for JWT token injection (Standalone compatible)
 */
export const jwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next) => {
  // Get token from localStorage directly to avoid service injection issues
  const token = localStorage.getItem('authToken');

  console.log('JwtInterceptor - URL:', req.url, 'Token:', !!token);

  if (token) {
    // Clone request and add Authorization header
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    console.log('Token added to request:', token.substring(0, 20) + '...');
  }

  return next(req);
};

/**
 * Class-based interceptor (kept for backward compatibility if needed)
 */
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    console.log('JwtInterceptor - URL:', request.url, 'Token:', !!token);

    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      console.log('Token added to request');
    }

    return next.handle(request);
  }
}
