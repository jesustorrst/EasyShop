import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'catalogo',
    pathMatch: 'full',
  },
  {
    path: 'catalogo',
    loadChildren: () => import('./features/catalog/catalog.routes').then((m) => m.catalogRoutes),
  },
  {
    path: '**',
    redirectTo: 'catalogo',
  },
];
