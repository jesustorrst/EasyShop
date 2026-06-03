import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product';
import { finalize, Observable, tap, of } from 'rxjs';
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

  createProduct(product: Product, imageFile: File | null) {
    this.#loading.set(true);

    const formData = new FormData();
    formData.append('name', product.name);
    formData.append('description', product.description);
    formData.append('price', product.price.toString());
    formData.append('categoryId', product.categoryId);

    if (imageFile) {
      formData.append('image', imageFile, imageFile.name);
    }

    return this.http.post<Product>(`${this.baseUrl}/product`, formData).pipe(
      tap((newProduct) => {
        console.log('📦 [Servicio Cache] Productos antes de actualizar:', this.#products());
        console.log('✨ [Servicio Cache] Producto nuevo devuelto por .NET:', newProduct);
        this.#products.update((products) => [...products, newProduct]);
        console.log('🚀 [Servicio Cache] Lista combinada final en memoria:', this.#products());
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

  getProductById(id: string): Observable<Product> {
    const existingProductInList = this.#products().find((p) => p.id === id);

    if (existingProductInList) {
      return of(existingProductInList);
    }

    this.#loading.set(true);

    return this.http
      .get<Product>(`${this.baseUrl}/product/${id}`)
      .pipe(finalize(() => this.#loading.set(false)));
  }
}
