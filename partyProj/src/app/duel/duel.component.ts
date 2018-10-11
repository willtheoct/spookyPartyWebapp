import { Component, OnInit, Input } from '@angular/core';
import { PartyGoer } from '../../models/partyGoer.model';
import { PlayerlistComponent } from '../playerlist/playerlist.component';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-duel',
  templateUrl: './duel.component.html',
  styleUrls: ['./duel.component.css']
})
export class DuelComponent {

  constructor(private http: HttpClient) { }
  target: PartyGoer = null;

  selectRef(r: PartyGoer) {
    this.http.put("duelChallenge", { body: { src: AppComponent.userId, target: this.target.id, referee: r.id } })
  }
  selectTarget(t: PartyGoer) {
    this.target = t;
    this.availableRefs = PartyGoer.onlinePlayers.filter(x => x.id !== t.id);
  }
  availableRefs: PartyGoer[];

}
