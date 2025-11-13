import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from '@angular/core';
import { Pagination } from "../../shared/models/pagination";
import { Book } from "../../shared/models/book";

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  pageSize = 20;

  private http = inject(HttpClient);
  categories: string[] = [];

  getBooks(categories?: string[], sort?: string) {
    let params = new HttpParams();

    if (categories && categories.length > 0) {
      params = params.append('categories', categories.join(','));
    }

    if (sort) {
      params = params.append('sort', sort); 
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
