import { Route, Routes } from '@angular/router';

export const catalogRoutes: Routes = [
  {
    path: 'productos',
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./products/product-list/product-list').then((m) => m.ProductList),
      },
      {
        path: 'add',
        loadComponent: () =>
          import('./products/product-form/product-form').then((m) => m.ProductForm),
      },
    ],
  },
  // ,{
  //   path: 'categories',
  //   children: [
  //     {
  //       path: '',
  //       // loadComponent: () => import('./categories/category-list/category-list').then(m => m.CategoryList)
  //     },
  //     {
  //       path: 'add',
  //       // loadComponent: () => import('./categories/category-form/category-form').then(m => m.CategoryForm)
  //     },
  //   ],
  // }
  {
    path: '',
    redirectTo: 'productos',
    pathMatch: 'full',
  },
];
