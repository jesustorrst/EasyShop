import { Component, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../../../shared/services/product.service';
import { ProductCard } from '../product-card/product-card';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [ProductCard, RouterModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  public productService = inject(ProductService);

  ngOnInit(): void {
    this.productService.getAllProducts();
  }
}
