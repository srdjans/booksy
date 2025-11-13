import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from "../../core/services/shop.service";
import { Book } from "../../shared/models/book";
import { BookItemComponent } from "./book-item/book-item.component";
import { MatDialog } from "@angular/material/dialog";
import { FiltersDialogComponent } from "./filters-dialog/filters-dialog.component";
import { MatButton } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";
import { MatMenu, MatMenuTrigger } from "@angular/material/menu";
import { MatListOption, MatSelectionList, MatSelectionListChange } from "@angular/material/list";

@Component({
  selector: 'app-shop',
  imports: [
    BookItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  books: Book[] = [];
  selectedCategories: string[] = [];
  selectedSort: string = 'name';
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'}
  ]

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getCategories();
    this.getBooks();
  }

  getBooks() {
    this.shopService.getBooks(this.selectedCategories, this.selectedSort).subscribe({
      next: response => this.books = response.data,
      error: error => console.log(error)
    })
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.selectedSort = selectedOption.value;
      this.getBooks();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedCategories: this.selectedCategories
      }
    });
    
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.selectedCategories = result.selectedCategories;
          this.getBooks();
        }
      }
    })
  }
}
