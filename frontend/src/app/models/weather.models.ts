// Weather data model
export interface WeatherData {
  city: string;
  temperature: number;
  humidity: number;
  description: string;
  icon: string;
  latitude: number;
  longitude: number;
  fromCache: boolean;
}

// Weather response model - matches the actual API response structure
export interface WeatherResponse extends WeatherData {
  success: boolean;
  message: string;
  data?: WeatherData;
}

// Weather request model
export interface WeatherRequest {
  city: string;
}
