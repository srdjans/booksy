import { Routes } from '@angular/router';
import { HomeComponent } from "./features/home/home.component";
import { ShopComponent } from "./features/shop/shop.component";
import { BookDetailsComponent } from "./features/shop/book-details/book-details.component";

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'shop', component: ShopComponent},
    {path: 'shop/:id', component: BookDetailsComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'}
];
