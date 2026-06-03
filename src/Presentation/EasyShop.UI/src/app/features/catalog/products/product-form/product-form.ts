import { Component, inject, OnInit, signal } from '@angular/core';
import { ProductService } from '../../../../shared/services/product.service';
import { CategoryService } from '../../../../shared/services/category.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { NotificationService } from '../../../../shared/services/notification.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.css',
})
export class ProductForm implements OnInit {
  private notificationService = inject(NotificationService);
  private formbuilder = inject(FormBuilder);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  public isSubmitting = false;

  public isEditMode = signal<boolean>(false);
  public productId = signal<string | null>(null);

  public imageSelected = signal<File | null>(null);
  public imagePreview = signal<string | null>(null);

  public categories = this.categoryService.categories;

  public fileErrorMessage = signal<string | null>(null);

  public productForm: FormGroup = this.formbuilder.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]],
    price: [0, [Validators.required, Validators.min(0)]],
    categoryId: ['', [Validators.required, Validators.minLength(3)]],
  });

  ngOnInit(): void {
    this.categoryService.getAllCategories();

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.isEditMode.set(true);
      this.productId.set(id);
      this.loadProduct(id);
    }
  }

  private loadProduct(id: string): void {
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        if (product) {
          this.productForm.patchValue({
            name: product.name,
            description: product.description,
            price: product.price,
            categoryId: product.categoryId,
          });
          if (product.imageUrl) {
            this.imagePreview.set(product.imageUrl);
          }
        }
      },
      error: (error) => {
        console.error('Error fetching product:', error);
        this.notificationService.showErrorNotification(
          'Error',
          'No se pudo obtener la información del producto.',
        );

        this.router.navigate(['/catalog/products']);
      },
    });
  }

  isFieldInvalid(field: string): boolean {
    const control = this.productForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.fileErrorMessage.set(null);

    if (input.files && input.files.length > 0) {
      const file = input.files[0];

      const validTypes = ['image/jpeg', 'image/png', 'image/gif'];

      if (!validTypes.includes(file.type)) {
        this.fileErrorMessage.set('Tipo de archivo no válido. Solo se permiten JPEG, PNG y GIF.');
        this.resetImageInput(input);
        return;
      }

      const maxSizeBytes = 1 * 1024 * 1024;
      if (file.size > maxSizeBytes) {
        this.fileErrorMessage.set('El archivo es demasiado grande. El tamaño máximo es de 5MB.');
        this.resetImageInput(input);
        return;
      }

      this.imageSelected.set(file);

      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview.set(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  }

  private resetImageInput(input: HTMLInputElement): void {
    input.value = '';
    this.imageSelected.set(null);
    this.imagePreview.set(null);
  }

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const productData = this.productForm.value;

    this.productService.createProduct(productData, this.imageSelected()).subscribe({
      next: () => {
        this.router.navigate(['/catalog/products']);
      },
      error: (error) => {
        console.error('Error creating product:', error);
        this.isSubmitting = false;
      },
    });
  }
}
