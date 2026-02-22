import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/auth.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginRequest: LoginRequest = { email: '', password: '' };
  loading: boolean = false;
  error: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    if (!this.loginRequest.email || !this.loginRequest.password) {
      this.error = 'Please fill in all fields';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.login(this.loginRequest).subscribe({
      next: (response) => {
        console.log('Login response:', response);
        if (response.success) {
          this.router.navigate(['/home']);
        } else {
          this.error = response.message || 'Login failed';
        }
      },
      error: (err) => {
        console.error('Login error:', err);
        this.error = err.error?.message || err.message || 'Login failed. Please try again.';
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  signInWithGoogle(): void {
    this.loading = true;
    this.error = '';

    // Declare Google as any to access window.google
    const google = (window as any).google;
    
    if (!google) {
      this.error = 'Google Sign-In not loaded. Please refresh the page.';
      this.loading = false;
      return;
    }

    console.log('Google library loaded, initializing...');

    // Initialize Google Sign-In with callback
    google.accounts.id.initialize({
      client_id: '542124379228-ghqsv4fuk82qm3v7otsj89cbb9j9oq66.apps.googleusercontent.com',
      callback: (response: any) => {
        console.log('Google callback triggered with response:', response);
        if (response && response.credential) {
          console.log('Credential found, handling sign-in...');
          this.handleGoogleSignIn(response);
        } else {
          console.log('No credential in response:', response);
          this.error = 'Google authentication failed - no credential received';
          this.loading = false;
        }
      }
    });

    // Use One Tap UI
    google.accounts.id.prompt((notification: any) => {
      console.log('Prompt notification:', notification);
      if (notification.isNotDisplayed()) {
        console.log('Prompt not displayed, rendering button');
        // Fallback: render button if prompt not shown
        const buttonContainer = document.getElementById('google-signin-btn');
        if (buttonContainer) {
          buttonContainer.innerHTML = ''; // Clear previous content
          google.accounts.id.renderButton(buttonContainer, {
            theme: 'outline',
            size: 'large',
            type: 'standard'
          });
        }
      }
    });
  }

  private handleGoogleSignIn(response: any): void {
    console.log('handleGoogleSignIn called with:', response);
    
    if (response.credential) {
      console.log('Credential exists, sending to backend...');
      // Send the ID token to backend for verification and login
      this.authService.googleLogin(response.credential).subscribe({
        next: (authResponse) => {
          console.log('✅ Google login successful:', authResponse);
          this.loading = false;
          this.router.navigate(['/home']);
        },
        error: (err) => {
          console.error('❌ Google login error:', err);
          this.loading = false;
          this.error = err.error?.message || err.message || 'Google login failed. Check backend.';
        }
      });
    } else {
      console.error('No credential in response');
      this.error = 'Google Sign-In failed. Please try again.';
      this.loading = false;
    }
  }
}
