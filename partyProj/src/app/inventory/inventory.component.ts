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

  @Input()
  selections: currency[] = Object.values(currencies).slice(Object.keys(currencies).length / 2).map(x => new currency(x, 0));

  constructor() { }

  ngOnInit() {
    if (isNullOrUndefined(this.player)) {
      this.player = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
    }
    console.log("showing " + this.player.characterName + "'s inventory with " + this.player.inventory.length + "currencies");
    for (let i = 0; i < (Object.keys(currencies).length / 2); i++) {
      const element = currencies[i];
      this.items[i] = new currency(i, this.player.inventory[i]);
    }
    console.log(this.items);
  }

  items: currency[] = [];
  currencyNameOf(c: currencies) {
    return currencies[c];
  }

  addItem(type: currencies) {
    this.selections[type].count++;
    this.selections[type].count = Math.min(this.selections[type].count, this.player.inventory[type]);
  }

  removeItem(type: currencies) {
    this.selections[type].count--;
    this.selections[type].count = Math.max(this.selections[type].count, 0);
    console.log(this.selections);
  }

}