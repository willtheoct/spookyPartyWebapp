import { Component, OnInit, Input, Output } from '@angular/core';
import { currency, currencies } from '../../models/currency.model';
import { playerIds, PartyGoer } from '../../models/partyGoer.model';
import { isNullOrUndefined } from 'util';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit {

  @Input()
  player: PartyGoer;

  @Output()
  selections: currency[] = [];

  constructor() { }

  ngOnInit() {
    if (isNullOrUndefined(this.player)) {
      this.player = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
    }
    this.items = this.player.inventory;
    console.log("inventory init");
  }

  items: currency[] = [];
  currencyNameOf(c: currencies) {
    return currencies[c];
  }

  selectItem(c: currency) {
    this.selections.push(c);
  }

}
