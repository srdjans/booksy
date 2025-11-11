import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from "../../core/services/shop.service";
import { Book } from "../../shared/models/book";
import { BookItemComponent } from "./book-item/book-item.component";
import { MatDialog } from "@angular/material/dialog";
import { FiltersDialogComponent } from "./filters-dialog/filters-dialog.component";
import { MatButton } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-shop',
  imports: [
    BookItemComponent,
    MatButton,
    MatIcon
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  books: Book[] = [];
  selectedAuthors: string[] = [];
  selectedCategories: string[] = [];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getAuthors();
    this.shopService.getCategories();
    this.shopService.getBooks().subscribe({
      next: response => this.books = response.data,
      error: error => console.log(error)
    })
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedAuthors: this.selectedAuthors,
        selectedCategories: this.selectedCategories
      }
    });
    
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          console.log(result);
          this.selectedAuthors = result.selectedAuthors;
          this.selectedCategories = result.selectedCategories;
        }
      }
    })
  }
}
