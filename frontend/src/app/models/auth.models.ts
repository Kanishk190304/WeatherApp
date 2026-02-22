// User authentication model
export interface User {
  id: number;
  email: string;
  name?: string;
  role: string;
  authProvider: string;
  profilePicture?: string;
  createdAt: Date;
}

// Login request model
export interface LoginRequest {
  email: string;
  password: string;
}

// Register request model
export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
}

// Authentication response model
export interface AuthResponse {
  success: boolean;
  message: string;
  user?: User;
  token?: string;
  expiresIn?: number;
}
