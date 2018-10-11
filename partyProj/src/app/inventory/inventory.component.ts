import { Component, OnInit } from '@angular/core';
import { currency, currencies } from '../../models/currency.model';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    let c = new currency();
    c.type = currencies.bronze;
    c.count = 29;
    this.items = [c, c, c, c, c, c, c]
  }

  items: currency[] = [];
  currencyNameOf(c: currencies) {
    return currencies[c];
  }

}
