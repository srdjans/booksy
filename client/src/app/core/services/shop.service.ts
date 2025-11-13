import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from '@angular/core';
import { Pagination } from "../../shared/models/pagination";
import { Book } from "../../shared/models/book";
import { ShopParams } from "../../shared/models/shopParams";

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  pageSize = 20;

  private http = inject(HttpClient);
  categories: string[] = [];

  getBooks(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.categories.length > 0) {
      params = params.append('categories', shopParams.categories.join(','));
    }

    if (shopParams.sort) {
      params = params.append('sort', shopParams.sort); 
    }

    params = params.append('pageSize', this.pageSize);

    return this.http.get<Pagination<Book>>(this.baseUrl + 'books', {params});
  }

  getCategories() {
    if (this.categories.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'books/categories').subscribe({
      next: response => this.categories = response
    });
  }
}
