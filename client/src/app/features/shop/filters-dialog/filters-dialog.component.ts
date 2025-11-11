import { Component, inject } from '@angular/core';
import { ShopService } from "../../../core/services/shop.service";
import { MatDivider } from "@angular/material/divider";
import { MatListOption, MatSelectionList } from "@angular/material/list";
import { MatButton } from "@angular/material/button";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'app-filters-dialog',
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss',
})
export class FiltersDialogComponent {
  protected shopService = inject(ShopService);
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  selectedAuthors: string[] = this.data.selectedAuthors;
  selectedCategories: string[] = this.data.selectedCategories;
  
  applyFilters() {
    this.dialogRef.close({
      selectedAuthors: this.selectedAuthors,
      selectedCategories: this.selectedCategories
    })
  }
}
