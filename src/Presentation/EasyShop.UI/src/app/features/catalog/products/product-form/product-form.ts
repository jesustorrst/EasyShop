import { Component, inject, OnInit } from '@angular/core';
import { ProductService } from '../../../../shared/services/product.service';
import { CategoryService } from '../../../../shared/services/category.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.css',
})
export class ProductForm implements OnInit {
  private formbuilder = inject(FormBuilder);
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private router = inject(Router);

  public isSubmitting = false;

  public categories = this.categoryService.categories;

  public productForm: FormGroup = this.formbuilder.group({
    name: ['', Validators.required, Validators.minLength(3), Validators.maxLength(100)],
    description: ['', [Validators.maxLength(500)]],
    price: [0, [Validators.required, Validators.min(0)]],
    categoryId: ['', [Validators.required, Validators.minLength(3)]],
  });

  ngOnInit(): void {
    this.categoryService.getAllCategories();
  }
  isFieldInvalid(field: string): boolean {
    const control = this.productForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const productData = this.productForm.value;

    this.productService.createProduct(productData).subscribe({
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
