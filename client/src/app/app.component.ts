import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from "@angular/common/http";
import { Book } from "./shared/models/book";
import { Pagination } from "./shared/models/pagination";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  title = 'Booksy';
  books: Book[] = [];

  ngOnInit(): void {
    this.http.get<Pagination<Book>>(this.baseUrl + 'books').subscribe({
      next: response => this.books = response.data,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
}
