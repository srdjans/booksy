import { Component, Input } from '@angular/core';
import { Book } from "../../../shared/models/book";
import { MatCard, MatCardActions, MatCardContent } from "@angular/material/card";
import { CurrencyPipe } from "@angular/common";
import { MatButton } from "@angular/material/button";
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-book-item',
  imports: [ 
    MatCard,
    MatCardContent,
    CurrencyPipe,
    MatCardActions,
    MatButton,
    MatIcon
  ],
  templateUrl: './book-item.component.html',
  styleUrl: './book-item.component.scss',
})
export class BookItemComponent {
  @Input() book?: Book;
}