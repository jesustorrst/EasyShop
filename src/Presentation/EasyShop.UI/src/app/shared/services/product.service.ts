import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product';
import { finalize, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  #products = signal<Product[]>([]);
  public products = this.#products.asReadonly();

  #loading = signal(false);
  public loading = this.#loading.asReadonly();

  getAllProducts(): void {
    if (this.#products().length > 0) return;

    this.#loading.set(true);

    this.http
      .get<Product[]>(`${this.baseUrl}/product`)
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: (data) => this.#products.set(data),
        error: (error) => {
          console.error('Error fetching products:', error);
        },
      });
  }

  createProduct(product: Product) {
    this.#loading.set(true);

    return this.http.post<Product>(`${this.baseUrl}/product`, product).pipe(
      tap((newProduct) => {
        this.#products.update((products) => [...products, newProduct]);
      }),
      finalize(() => this.#loading.set(false)),
    );
  }

  deleteProduct(id: string): Observable<void> {
    this.#loading.set(true);

    return this.http.delete<void>(`${this.baseUrl}/product/${id}`).pipe(
      tap(() => {
        this.#products.update((current) => current.filter((p) => p.id !== id));
      }),
      finalize(() => this.#loading.set(false)),
    );
  }
}
