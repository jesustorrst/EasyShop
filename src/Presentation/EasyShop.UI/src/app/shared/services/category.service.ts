import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category';
import { finalize, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  #categories = signal<Category[]>([]);
  public categories = this.#categories.asReadonly();

  #loading = signal(false);
  public loading = this.#loading.asReadonly();

  getAllCategories(): void {
    if (this.#categories().length > 0) return;

    this.#loading.set(true);

    this.http
      .get<Category[]>(`${this.baseUrl}/category`)
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: (data) => this.#categories.set(data),
        error: (error) => {
          console.error('Error fetching categories:', error);
        },
      });
  }
}
