import { Component, OnInit } from '@angular/core';
import { playerIds, PartyGoer } from '../../models/partyGoer.model';
import { currencies, currency } from '../../models/currency.model';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-trade',
  templateUrl: './trade.component.html',
  styleUrls: ['./trade.component.css']
})
export class TradeComponent implements OnInit {
  target: PartyGoer;
  players = [];
  MyTradeOffer: currency[] = [];
  TheirTradeOffer: currency[] = [];
  selectTarget(player: PartyGoer) {
    console.log(player);
    this.target = player;
  }
  ngOnInit() {
    this.players = PartyGoer.onlinePlayers;
  }

}
