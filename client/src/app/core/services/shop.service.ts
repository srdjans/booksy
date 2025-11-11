import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from '@angular/core';
import { Pagination } from "../../shared/models/pagination";
import { Book } from "../../shared/models/book";

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  categories: string[] = [];
  authors: string[] = [];

  getBooks() {
    return this.http.get<Pagination<Book>>(this.baseUrl + 'books?pageSize=20');
  }

  getAuthors() {
    if (this.authors.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'books/authors').subscribe({
      next: response => this.authors = response
    });
  }

  getCategories() {
    if (this.categories.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'books/categories').subscribe({
      next: response => this.categories = response
    });
  }
}
