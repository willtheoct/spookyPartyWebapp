import { Component, OnInit, Input, Output } from '@angular/core';
import { PartyGoer } from '../../models/partyGoer.model';
import { PlayerlistComponent } from '../playerlist/playerlist.component';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { currency } from '../../models/currency.model';
import { interval } from 'rxjs/observable/interval';

@Component({
  selector: 'app-duel',
  templateUrl: './duel.component.html',
  styleUrls: ['./duel.component.css']
})
export class DuelComponent implements OnInit {
  ngOnInit(): void {
    this.availableDuelists = PartyGoer.onlinePlayers.filter(x => x.id !== AppComponent.userId);
    this.thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
    interval(6000).subscribe((v) => {
      let x = ["Smash Bros?", "Go Fish?", "Swords?", "Staring Contest?", "Rock Paper Scissors?", "Chess?", "Fisticuffs?", "Chicken?", "Yu-Gi-Oh!?"];
      this.randomPlaceholder = "";
      for (let i = 0; i < 3; i++) {
        let y = x.splice(Math.round(Math.random() * (x.length - 1)), 1);
        this.randomPlaceholder += y + " ";
      }
    });
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
  randomPlaceholder = "";

}
