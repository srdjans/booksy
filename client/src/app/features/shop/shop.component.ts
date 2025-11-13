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
import { ShopParams } from "../../shared/models/shopParams";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Pagination } from "../../shared/models/pagination";

@Component({
  selector: 'app-shop',
  imports: [
    BookItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  books?: Pagination<Book>;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'}
  ]
  shopParams = new ShopParams();
  pageSizeOptions = [5, 10, 15, 20];

  ngOnInit(): void {
    this.initializeShop();
  }

  initializeShop() {
    this.shopService.getCategories();
    this.getBooks();
  }

  getBooks() {
    this.shopService.getBooks(this.shopParams).subscribe({
      next: response => this.books = response,
      error: error => console.log(error)
    })
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getBooks();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getBooks();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedCategories: this.shopParams.categories
      }
    });
    
    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          this.shopParams.categories = result.selectedCategories;
          this.shopParams.pageNumber = 1;
          this.getBooks();
        }
      }
    })
  }
}
