import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { WeatherService } from '../../services/weather.service';
import { WeatherData } from '../../models/weather.models';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  city: string = '';
  weatherData: WeatherData | null = null;
  loading: boolean = false;
  error: string = '';
  mapUrl: SafeResourceUrl = '';

  constructor(
    private weatherService: WeatherService,
    private cdr: ChangeDetectorRef,
    private sanitizer: DomSanitizer
  ) {}

  searchWeather(): void {
    if (!this.city.trim()) {
      this.error = 'Please enter a city name';
      return;
    }

    this.loading = true;
    this.error = '';
    this.weatherData = null;

    console.log('Searching for city:', this.city);
    console.log('Token available:', !!localStorage.getItem('authToken'));

    this.weatherService.getWeatherByCity(this.city)
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.markForCheck();
          console.log('Loading set to false, change detection triggered');
        })
      )
      .subscribe({
        next: (response) => {
          console.log('Weather response received:', response);
          if (response.success) {
            // Response contains the weather data at the root level, not in a nested 'data' property
            this.weatherData = {
              city: response.city || '',
              temperature: response.temperature || 0,
              humidity: response.humidity || 0,
              description: response.description || '',
              icon: response.icon || '',
              latitude: response.latitude || 0,
              longitude: response.longitude || 0,
              fromCache: response.fromCache || false
            };
            console.log('Weather data set:', this.weatherData);
            
            // Generate embedded map URL
            this.generateMapUrl(this.weatherData.latitude, this.weatherData.longitude);
          } else {
            this.error = response.message || 'No weather data found';
          }
        },
        error: (err) => {
          console.error('Weather error:', err);
          this.error = err.error?.message || err.message || `Failed to fetch weather data (${err.status})`;
        }
      });
  }

  openMap(): void {
    if (this.weatherData) {
      const lat = this.weatherData.latitude;
      const lon = this.weatherData.longitude;
      const url = `https://www.openstreetmap.org/?mlat=${lat}&mlon=${lon}&zoom=12`;
      window.open(url, '_blank');
    }
  }

  generateMapUrl(latitude: number, longitude: number): void {
    // Use OpenStreetMap with Leaflet iframe embed (no API key needed)
    const embedUrl = `https://www.openstreetmap.org/export/embed.html?bbox=${longitude - 0.05},${latitude - 0.05},${longitude + 0.05},${latitude + 0.05}&layer=mapnik&marker=${latitude},${longitude}`;
    this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
  }
}
