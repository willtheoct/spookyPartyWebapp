import { Component, OnInit, Input, Output } from '@angular/core';
import { PartyGoer } from '../../models/partyGoer.model';
import { PlayerlistComponent } from '../playerlist/playerlist.component';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { currency } from '../../models/currency.model';

@Component({
  selector: 'app-duel',
  templateUrl: './duel.component.html',
  styleUrls: ['./duel.component.css']
})
export class DuelComponent implements OnInit {
  ngOnInit(): void {
    this.availableDuelists = PartyGoer.onlinePlayers.filter(x => x.id !== AppComponent.userId);
    this.thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
    console.log(AppComponent.userId);
    console.log(PartyGoer.onlinePlayers.map(x => x.id));
  }

  constructor(private http: HttpClient) { }
  target: PartyGoer = null;
  referee: PartyGoer = null;
  thisPlayer: PartyGoer = null;

  selectRef(r: PartyGoer) {
    this.http.put("duelChallenge", { body: { src: AppComponent.userId, target: this.target.id, referee: r.id } })
  }
  selectTarget(t: PartyGoer) {
    this.target = t;
    this.availableRefs = this.availableDuelists.filter(x => x.id !== t.id);
  }
  availableRefs: PartyGoer[];
  availableDuelists: PartyGoer[];
  description: string = '';
  targetInventoryWager: currency[] = [];
  myInventoryWager: currency[] = [];

}
