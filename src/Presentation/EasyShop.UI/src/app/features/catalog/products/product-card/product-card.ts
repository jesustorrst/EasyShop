import { Component, input, inject } from '@angular/core';
import { Product } from '../../../../shared/models/product';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../../shared/services/product.service';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {
  private productService = inject(ProductService);

  product = input.required<Product>();

  onDelete(id: string | undefined): void {
    if (!id) return;

    const confirmed = confirm('¿Estás seguro de que deseas eliminar este producto?');
    if (confirmed) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          alert('Producto eliminado exitosamente.');
        },
        error: (error) => {
          console.error('Error eliminando producto:', error);
          alert('Ocurrió un error al eliminar el producto. Por favor, intenta nuevamente.');
        },
      });
    }
  }
}
