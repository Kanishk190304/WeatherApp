import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { WeatherRequest, WeatherResponse, WeatherData } from '../models/weather.models';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  constructor(private http: HttpClient) {}

  /**
   * Get weather data for a city
   */
  getWeatherByCity(city: string): Observable<WeatherResponse> {
    const request: WeatherRequest = { city };
    return this.http.post<WeatherResponse>(`${environment.weatherServiceUrl}/get-weather`, request);
  }

  /**
   * Get weather by coordinates
   */
  getWeatherByCoordinates(latitude: number, longitude: number): Observable<WeatherResponse> {
    return this.http.get<WeatherResponse>(
      `${environment.weatherServiceUrl}/get-weather-by-coords?lat=${latitude}&lon=${longitude}`
    );
  }
}
