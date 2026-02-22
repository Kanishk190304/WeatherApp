import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/auth.models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerRequest: RegisterRequest = { email: '', password: '', confirmPassword: '' };
  loading: boolean = false;
  error: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  register(): void {
    if (!this.registerRequest.email || !this.registerRequest.password || !this.registerRequest.confirmPassword) {
      this.error = 'Please fill in all fields';
      return;
    }

    if (this.registerRequest.password !== this.registerRequest.confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.register(this.registerRequest).subscribe({
      next: (response) => {
        console.log('Register response:', response);
        if (response.success) {
          this.router.navigate(['/home']);
        } else {
          this.error = response.message || 'Registration failed';
        }
      },
      error: (err) => {
        console.error('Register error:', err);
        this.error = err.error?.message || err.message || 'Registration failed. Please try again.';
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
